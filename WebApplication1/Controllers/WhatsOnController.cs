using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class WhatsOnController : Controller
    {
        private readonly MuseumDbContext _db;

        public WhatsOnController(MuseumDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Tours()
        {
            return View();
        }

        public IActionResult Booking()
        {
            return View();
        }

        public IActionResult Ticket()
        {
            return View();
        }

        // Events listing page - loads all events from SQL
        public async Task<IActionResult> Events()
        {
            var events = await _db.Events.OrderByDescending(e => e.StartDate).ToListAsync();
            return View("~/Views/Events/Events.cshtml", events);
        }

        // Individual event detail page
        public async Task<IActionResult> EventDetail(int id)
        {
            var ev = await _db.Events.FindAsync(id);
            if (ev == null) return NotFound();
            return View("~/Views/Events/EventDetail.cshtml", ev);
        }

        // Event booking page (GET)
        public async Task<IActionResult> EventBook(int id)
        {
            var ev = await _db.Events.FindAsync(id);
            if (ev == null) return NotFound();
            return View("~/Views/Events/EventBook.cshtml", ev);
        }

        // Event booking page (POST - saves to database)
        [HttpPost]
        public async Task<IActionResult> EventBook(int id, string bookingDate, string bookingTime, int quantity, string email, string? phone)
        {
            var ev = await _db.Events.FindAsync(id);
            if (ev == null) return NotFound();

            var chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var random = new Random();
            var code = "NG-";
            for (int i = 0; i < 12; i++)
            {
                if (i > 0 && i % 4 == 0) code += "-";
                code += chars[random.Next(chars.Length)];
            }

            var booking = new Booking
            {
                EventId = id,
                TicketCode = code,
                BookingDate = bookingDate,
                BookingTime = bookingTime,
                Quantity = quantity,
                Email = email,
                Phone = phone,
                CreatedAt = DateTime.UtcNow
            };

            _db.Bookings.Add(booking);
            await _db.SaveChangesAsync();

            return RedirectToAction("EventTicket", new { bookingId = booking.BookingId });
        }

        // Ticket page - loads from database
        public async Task<IActionResult> EventTicket(int bookingId)
        {
            var booking = await _db.Bookings
                .Include(b => b.Event)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);
            if (booking == null) return NotFound();
            return View("~/Views/Events/EventTicket.cshtml", booking);
        }

        // Keep old routes working
        public async Task<IActionResult> Event1() => await EventDetail(1);
        public async Task<IActionResult> Event2() => await EventDetail(2);
        public async Task<IActionResult> Event3() => await EventDetail(3);
        public async Task<IActionResult> Event4() => await EventDetail(4);
        public async Task<IActionResult> Event5() => await EventDetail(5);
        public async Task<IActionResult> Event6() => await EventDetail(6);
    }
}