using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    // Handles all event-related pages
    public class WhatsOnController : Controller
    {
        private readonly MuseumDbContext _db;

        public WhatsOnController(MuseumDbContext db)
        {
            _db = db;
        }

        public IActionResult Index() => View();
        public IActionResult Tours() => View();
        public IActionResult Booking() => View();
        public IActionResult Ticket() => View();

        // EVENTS LISTING — loads all events sorted newest first
        public async Task<IActionResult> Events()
        {
            var events = await _db.Events
                .OrderByDescending(e => e.StartDate)
                .ToListAsync();

            return View("~/Views/Events/Events.cshtml", events);
        }

        // EVENT DETAIL — shows full info for one event
        public async Task<IActionResult> EventDetail(int id)
        {
            var ev = await _db.Events.FindAsync(id);
            if (ev == null) return NotFound();
            return View("~/Views/Events/EventDetail.cshtml", ev);
        }

        // BOOKING FORM (GET)
        public async Task<IActionResult> EventBook(int id)
        {
            var ev = await _db.Events.FindAsync(id);
            if (ev == null) return NotFound();
            return View("~/Views/Events/EventBook.cshtml", ev);
        }

        // BOOKING FORM (POST) — validates, checks availability, saves booking + ticket
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EventBook(
            int id, string bookingDate, string bookingTime,
            int quantity, string email, string? phone)
        {
            var ev = await _db.Events.FindAsync(id);
            if (ev == null) return NotFound();

            if (string.IsNullOrWhiteSpace(bookingDate) || string.IsNullOrWhiteSpace(bookingTime))
            {
                TempData["Error"] = "Please select a date and time.";
                return RedirectToAction("EventBook", new { id });
            }

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                TempData["Error"] = "Please enter a valid email address.";
                return RedirectToAction("EventBook", new { id });
            }

            if (quantity < 1 || quantity > 5)
            {
                TempData["Error"] = "Quantity must be between 1 and 5.";
                return RedirectToAction("EventBook", new { id });
            }

            var alreadyBooked = await _db.EventBookings
                .Where(b => b.EventId == id
                         && b.BookingDate == bookingDate
                         && b.BookingTime == bookingTime)
                .SumAsync(b => b.Quantity);

            int spotsRemaining = ev.SpotsPerSlot - alreadyBooked;

            if (quantity > spotsRemaining)
            {
                TempData["Error"] = $"Only {spotsRemaining} spot(s) remaining for this time slot.";
                return RedirectToAction("EventBook", new { id });
            }

            // Generate unique ticket code (format: NG-XXXX-XXXX-XXXX)
            var chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var random = new Random();
            var code = "NG-";
            for (int i = 0; i < 12; i++)
            {
                if (i > 0 && i % 4 == 0) code += "-";
                code += chars[random.Next(chars.Length)];
            }

            // Get logged-in user's ID (null if guest)
            int? userId = HttpContext.Session.GetInt32("UserId");

            // Save the booking
            var booking = new EventBooking
            {
                EventId = id,
                UserId = userId,
                TicketCode = code,
                BookingDate = bookingDate,
                BookingTime = bookingTime,
                Quantity = quantity,
                Email = email,
                Phone = phone,
                CreatedAt = DateTime.UtcNow
            };

            _db.EventBookings.Add(booking);
            await _db.SaveChangesAsync();

            // If logged in, also create a Ticket record so it shows on Membership page
            if (userId.HasValue)
            {
                var ticket = new Ticket
                {
                    UserId = userId.Value,
                    EventBookingId = booking.EventBookingId,
                    CreatedAt = DateTime.UtcNow
                };
                _db.Tickets.Add(ticket);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("EventTicket", new { bookingId = booking.EventBookingId });
        }

        // TOGGLE FAVOURITE (POST) — adds or removes a saved event for logged-in users
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFavourite(int eventId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                TempData["Error"] = "Please log in to save favourites.";
                return RedirectToAction("EventDetail", new { id = eventId });
            }

            // Check if already favourited
            var existing = await _db.Favourites
                .FirstOrDefaultAsync(f => f.UserId == userId.Value && f.EventId == eventId);

            if (existing != null)
            {
                // Remove favourite (un-heart)
                _db.Favourites.Remove(existing);
                TempData["Success"] = "Removed from favourites.";
            }
            else
            {
                // Add favourite (heart)
                _db.Favourites.Add(new Favourite
                {
                    UserId = userId.Value,
                    EventId = eventId,
                    CreatedAt = DateTime.UtcNow
                });
                TempData["Success"] = "Added to favourites!";
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("EventDetail", new { id = eventId });
        }

        // CHECK IF FAVOURITED (for views to show filled/empty heart)
        [HttpGet]
        public async Task<IActionResult> IsFavourited(int eventId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return Json(new { favourited = false });

            var exists = await _db.Favourites
                .AnyAsync(f => f.UserId == userId.Value && f.EventId == eventId);

            return Json(new { favourited = exists });
        }

        // AVAILABLE SPOTS API
        [HttpGet]
        public async Task<IActionResult> GetAvailableSpots(int eventId, string date, string time)
        {
            var ev = await _db.Events.FindAsync(eventId);
            if (ev == null) return Json(new { spots = 0, total = 0 });

            var booked = await _db.EventBookings
                .Where(b => b.EventId == eventId
                         && b.BookingDate == date
                         && b.BookingTime == time)
                .SumAsync(b => b.Quantity);

            return Json(new { spots = ev.SpotsPerSlot - booked, total = ev.SpotsPerSlot });
        }

        // TICKET CONFIRMATION
        public async Task<IActionResult> EventTicket(int bookingId)
        {
            var booking = await _db.EventBookings
                .Include(b => b.Event)
                .FirstOrDefaultAsync(b => b.EventBookingId == bookingId);

            if (booking == null) return NotFound();
            return View("~/Views/Events/EventTicket.cshtml", booking);
        }

        // SEARCH API
        [HttpGet]
        public async Task<IActionResult> SearchEvents(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Json(new List<object>());

            string term = query.Trim().ToLower();

            var results = await _db.Events
                .Where(e =>
                    e.Title.ToLower().Contains(term) ||
                    e.Description.ToLower().Contains(term) ||
                    e.Genre.ToLower().Contains(term) ||
                    e.Location.ToLower().Contains(term) ||
                    e.EventType.ToLower().Contains(term))
                .Select(e => new
                {
                    e.EventId,
                    e.Title,
                    e.EventType,
                    e.Genre,
                    e.ImagePath,
                    e.Location
                })
                .Take(6)
                .ToListAsync();

            return Json(results);
        }

        // Legacy route aliases
        public async Task<IActionResult> Event1() => await EventDetail(1);
        public async Task<IActionResult> Event2() => await EventDetail(2);
        public async Task<IActionResult> Event3() => await EventDetail(3);
        public async Task<IActionResult> Event4() => await EventDetail(4);
        public async Task<IActionResult> Event5() => await EventDetail(5);
        public async Task<IActionResult> Event6() => await EventDetail(6);
    }
}