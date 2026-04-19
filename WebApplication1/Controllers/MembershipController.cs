using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    public class MembershipController : Controller
    {
        private readonly MuseumDbContext _db;

        public MembershipController(MuseumDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            // If already logged in, skip Sign Up/Login page and go straight to Account
            if (HttpContext.Session.GetString("IsLoggedIn") == "true")
            {
                return RedirectToAction("Account", "Account");
            }
            return View();
        }

        public IActionResult Favourites()
        {
            return View();
        }

        public IActionResult Tickets()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return View(); // Show login prompt
            }

            // Get all tickets for this user (both events and tours)
            var tickets = _db.Tickets
                .Where(t => t.UserId == userId.Value)
                .Include(t => t.EventBooking)
                .ThenInclude(b => b!.Event)  
                .Include(t => t.TourBooking)
                .ThenInclude(b => b!.Tour)   
                .OrderByDescending(t => t.CreatedAt)
                .ToList();

            return View(tickets);
        }
    }
}