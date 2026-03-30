using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        // Login Page
        public IActionResult Index()
        {
            return View("~/Views/Account/Login.cshtml");
        }

        // Handle Login POST
        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            // Add your authentication logic here
            return RedirectToAction("Account", "Account");
        }

        // Register Page
        public IActionResult Register()
        {
            return View();
        }

        // Handle Register POST
        [HttpPost]
        public IActionResult Register(string username, string email, string password)
        {
            // Add your registration logic here
            return RedirectToAction("Index", "Login");
        }
    }
}