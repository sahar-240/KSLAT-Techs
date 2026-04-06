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