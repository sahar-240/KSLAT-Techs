using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class AmenitiesController : Controller
    {
        // GET: Amenities/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: Amenities/Privacy
        public IActionResult Privacy()
        {
            return View("Index"); // Returns the same view but shows privacy section
        }

        // GET: Amenities/Membership
        public IActionResult Membership()
        {
            // Redirect to Membership page (you'll create this later)
            return RedirectToAction("Index", "Membership");
        }

        // POST: Amenities/JoinMembership
        [HttpPost]
        public IActionResult JoinMembership()
        {
            return RedirectToAction("Index", "Membership");
        }
    }
}