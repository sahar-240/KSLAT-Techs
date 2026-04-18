using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly MuseumDbContext _db;

        public AccountController(MuseumDbContext db)
        {
            _db = db;
        }

        // Account/Membership Page
        public IActionResult Account()
        {
            // If not logged in, redirect to login
            if (HttpContext.Session.GetString("IsLoggedIn") != "true")
            {
                return RedirectToAction("Index", "Login");
            }
            return View("~/Views/Account/Account.cshtml");
        }

        // SAVED/FAVOURITES — loads the user's saved events from the database
        public async Task<IActionResult> Saved()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                TempData["ErrorMessage"] = "Please log in to view your saved items.";
                return RedirectToAction("Index", "Login");
            }

            var savedItems = await _db.Favourites
       .Where(f => f.UserId == userId.Value &&
                   (f.EventId != null || f.TourId != null))
       .Include(f => f.Event)
       .Include(f => f.Tour)
       .Select(f => new SavedItemViewModel
       {
           Id = f.FavouriteId,
           Title = f.Event != null ? f.Event.Title : (f.Tour != null ? f.Tour.Title : ""),
           Description = f.Event != null ? f.Event.Description : (f.Tour != null ? f.Tour.Description : ""),
           Date = f.Event != null
               ? $"{f.Event.StartDate:dd MMM yyyy} – {f.Event.EndDate:dd MMM yyyy}"
               : (f.Tour != null ? $"{f.Tour.StartDate:dd MMM yyyy} – {f.Tour.EndDate:dd MMM yyyy}" : ""),
           ImagePath = f.Event != null ? f.Event.ImagePath : (f.Tour != null ? f.Tour.ImagePath : "")
       })
       .ToListAsync();

            return View("~/Views/Account/Saved.cshtml", savedItems);
        }

        // TICKETS — loads the user's booked tickets from the database
        public async Task<IActionResult> Tickets()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                TempData["ErrorMessage"] = "Please log in to view your tickets.";
                return RedirectToAction("Index", "Login");
            }

            // --- Event tickets ---
            var eventTickets = await _db.Tickets
                .Where(t => t.UserId == userId.Value && t.EventBookingId != null)
                .Include(t => t.EventBooking!).ThenInclude(eb => eb.Event)
                .Select(t => new TicketViewModel
                {
                    Id = t.EventBookingId!.Value,
                    Title = t.EventBooking!.Event!.Title,
                    Description = t.EventBooking.Event.Location,
                    Date = t.EventBooking.BookingDate + " at " + t.EventBooking.BookingTime,
                    Status = "Confirmed",
                    Quantity = t.EventBooking.Quantity,
                    Price = 0,
                    TotalPrice = 0,
                    TicketCode = t.EventBooking.TicketCode,
                    BookingTime = t.EventBooking.BookingTime,
                    Email = t.EventBooking.Email,
                    
                })
                .ToListAsync();

            // --- Tour tickets ---
            var tourTickets = await _db.Tickets
                .Where(t => t.UserId == userId.Value && t.TourBookingId != null)
                .Include(t => t.TourBooking!).ThenInclude(tb => tb.Tour)
                .Select(t => new TicketViewModel
                {
                    Id = t.TourBookingId!.Value,
                    Title = t.TourBooking!.Tour!.Title,
                    Description = t.TourBooking.Tour.Location,
                    Date = t.TourBooking.BookingDate + " at " + t.TourBooking.BookingTime,
                    Status = "Confirmed",
                    Quantity = t.TourBooking.Quantity,
                    Price = t.TourBooking.Price,
                    TotalPrice = t.TourBooking.TotalPrice,
                    TicketCode = t.TourBooking.TicketCode,
                    BookingTime = t.TourBooking.BookingTime,
                    Email = t.TourBooking.Email,
                    CardholderName = t.TourBooking.CardholderName
                })
                .ToListAsync();

            // --- Combine both ---
            var allTickets = eventTickets.Concat(tourTickets)
                .OrderByDescending(t => t.Date)
                .ToList();

            return View("~/Views/Account/Tickets.cshtml", allTickets);
        }
        // REMOVE FAVOURITE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFavourite(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Index", "Login");

            var fav = await _db.Favourites
                .FirstOrDefaultAsync(f => f.FavouriteId == id && f.UserId == userId.Value);

            if (fav != null)
            {
                _db.Favourites.Remove(fav);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Removed from favourites.";
            }

            return RedirectToAction("Saved");
        }

        // CANCEL TICKET
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelTicket(int ticketId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Index", "Login");

            // Find the ticket and its associated booking
            var ticket = await _db.Tickets
                .Include(t => t.EventBooking)
                .FirstOrDefaultAsync(t => t.EventBookingId == ticketId && t.UserId == userId.Value);

            if (ticket != null)
            {
                // Remove the booking itself (frees up the spot)
                if (ticket.EventBooking != null)
                {
                    _db.EventBookings.Remove(ticket.EventBooking);
                }
                // Remove the ticket record
                _db.Tickets.Remove(ticket);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Booking cancelled successfully.";
            }

            return RedirectToAction("Tickets");
        }
    }
}