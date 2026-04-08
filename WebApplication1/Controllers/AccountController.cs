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
                .Where(f => f.UserId == userId.Value && f.EventId != null)
                .Include(f => f.Event)
                .Select(f => new SavedItemViewModel
                {
                    Id = f.FavouriteId,
                    Title = f.Event!.Title,
                    Description = f.Event.Description,
                    Date = f.Event.StartDate.ToString("dd MMM yyyy") + " – " + f.Event.EndDate.ToString("dd MMM yyyy"),
                    ImagePath = f.Event.ImagePath
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

            var tickets = await _db.Tickets
                .Where(t => t.UserId == userId.Value && t.EventBookingId != null)
                .Include(t => t.EventBooking!)
                    .ThenInclude(eb => eb.Event)
                .Select(t => new TicketViewModel
                {
                    Id = t.EventBookingId!.Value,
                    Title = t.EventBooking!.Event!.Title,
                    Description = t.EventBooking.Event.Location,
                    Date = t.EventBooking.BookingDate + " at " + t.EventBooking.BookingTime,
                    Status = "Confirmed",
                    Quantity = t.EventBooking.Quantity,
                    Price = 0,
                    TicketCode = t.EventBooking.TicketCode
                })
                .ToListAsync();

            return View("~/Views/Account/Tickets.cshtml", tickets);
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