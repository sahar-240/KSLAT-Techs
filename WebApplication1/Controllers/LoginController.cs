using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            // Add your login logic here
            // For now, this is a placeholder
            if (ValidateCredentials(username, password))
            {
                HttpContext.Session.SetString("Username", username);
                HttpContext.Session.SetString("IsLoggedIn", "true");

                // Set success message
                TempData["SuccessMessage"] = $"Welcome back, {username}!";

                return RedirectToAction("Account", "Account");
             
            }

            TempData["ErrorMessage"] = "Invalid username or password. Please try again.";
            ModelState.AddModelError("", "Invalid username or password");
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Clear session
            HttpContext.Session.Clear();

            // Set logout message
            TempData["SuccessMessage"] = "You have been logged out successfully.";

            return RedirectToAction("Index", "Home");
        }

        private static bool ValidateCredentials(string username, string password)
        {
            // Add your authentication logic here
            // This is a placeholder - replace with actual database validation

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            // TODO: Replace with your actual user validation from database
            // Example:
            // var user = _userRepository.GetUser(username);
            // return user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            // For demonstration purposes
            return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
        }
    }
}