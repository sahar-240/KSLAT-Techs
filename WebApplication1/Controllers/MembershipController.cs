using Microsoft.AspNetCore.Mvc;
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
            return View();
        }
    }
}