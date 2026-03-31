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

        // BUG FIX: Added missing Privacy action (linked from Amenities.cshtml header and footer)
        public IActionResult Privacy()
        {
            return View("~/Views/Amenities/Amenities.cshtml");
        }

        // BUG FIX: Added missing JoinMembership POST action (form in Amenities.cshtml)
        [HttpPost]
        public IActionResult JoinMembership()
        {
            return RedirectToAction("Membership");
        }

        // BUG FIX: Added missing Membership action (linked from Membership.cshtml nav)
        public IActionResult Membership()
        {
            return View("~/Views/Membership/Membership.cshtml");
        }

        // BUG FIX: Added missing SignUp action (button in Membership.cshtml)
        public IActionResult SignUp()
        {
            return View("~/Views/SignUp/SignUp.cshtml");
        }
    }
}
