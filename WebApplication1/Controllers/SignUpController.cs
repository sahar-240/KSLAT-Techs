using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class SignUpController : Controller
    {
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(string title, string firstName, string lastName, string email,
                                    string phone, string username, string address, string city,
                                    string county, string postcode)
        {
            // Basic validation
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username))
            {
                ModelState.AddModelError("", "Please fill in all required fields.");
                return View();
            }

            // Validate email format
            if (!email.Contains("@"))
            {
                ModelState.AddModelError("", "Please enter a valid email address.");
                return View();
            }

            // Set success message using TempData
            TempData["SuccessMessage"] = $"Welcome {firstName}! Sign up successful. Please log in with your username.";

            // Redirect to login page
            return RedirectToAction("Index", "Login");
        }
    }
}