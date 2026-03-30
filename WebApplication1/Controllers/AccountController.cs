using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        // Account/Membership Page
        public IActionResult Account()
        {
            return View("~/Views/Account/Account.cshtml");
        }

        // Saved Page
        public IActionResult Saved()
        {
            return View("~/Views/Account/Saved.cshtml");
        }

        // Tickets Page
        public IActionResult Tickets()
        {
            return View("~/Views/Account/Tickets.cshtml");
        }
    }
}