using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        // Account/Membership Page
        public IActionResult Account()
        {
            return View("~/Views/Account/Account.cshtml");
        }

        // BUG FIX: Pass an empty list so Saved.cshtml doesn't throw NullReferenceException
        public IActionResult Saved()
        {
            var savedItems = new List<SavedItemViewModel>(); // TODO: load from database
            return View("~/Views/Account/Saved.cshtml", savedItems);
        }

        // BUG FIX: Pass an empty list so Tickets.cshtml doesn't throw NullReferenceException
        public IActionResult Tickets()
        {
            var tickets = new List<TicketViewModel>(); // TODO: load from database
            return View("~/Views/Account/Tickets.cshtml", tickets);
        }
    }
}
