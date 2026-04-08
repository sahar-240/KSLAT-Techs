using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    // Handles user login and logout.
    // Checks credentials against the Users table and sets session variables.
    public class LoginController : Controller
    {
        private readonly MuseumDbContext _db;

        public LoginController(MuseumDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // If already logged in, go straight to Account page
            if (HttpContext.Session.GetString("IsLoggedIn") == "true")
            {
                return RedirectToAction("Account", "Account");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                TempData["ErrorMessage"] = "Please enter both username and password.";
                return View();
            }

            // Hash the entered password the same way SignUp does
            var hash = Convert.ToBase64String(
                System.Security.Cryptography.SHA256.HashData(
                    System.Text.Encoding.UTF8.GetBytes(password)));

            // Look up the user by username AND password hash
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == hash);

            if (user == null)
            {
                TempData["ErrorMessage"] = "Invalid username or password. Please try again.";
                return View();
            }

            // Set session variables so the rest of the site knows who is logged in
            HttpContext.Session.SetString("IsLoggedIn", "true");
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("FirstName", user.FirstName);
            HttpContext.Session.SetString("UserEmail", user.Email);

            TempData["SuccessMessage"] = $"Welcome back, {user.FirstName}!";
            return RedirectToAction("Account", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Index", "Home");
        }
    }
}