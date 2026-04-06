using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class AmenitiesController : Controller
    {
        // FIX: Index() must use explicit path — file is Amenities.cshtml not Index.cshtml
        public IActionResult Index()
        {
            return View();
        }

        // Privacy page — same view, JS toggles the privacy div
        public IActionResult Privacy()
        {
            return View("~/Views/Amenities/Index.cshtml");
        }

        // JOIN button on Membership page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult JoinMembership()
        {
            return RedirectToAction("Membership");
        }

        // Membership page
        public IActionResult Membership()
        {
            return View("~/Views/Membership/Membership.cshtml");
        }

        // Sign Up page
        public IActionResult SignUp()
        {
            return View("~/Views/SignUp/SignUp.cshtml");
        }
    }
}