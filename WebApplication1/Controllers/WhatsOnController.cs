using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class WhatsOnController : Controller
    {
        // --- What's On landing page ---
        public IActionResult Index()
        {
            return View();
        }

        // --- Tours page (someone else's) ---
        public IActionResult Tours()
        {
            return View();
        }

        // --- Tours booking (someone else's) ---
        public IActionResult Booking()
        {
            return View();
        }

        // --- Tours ticket (someone else's) ---
        public IActionResult Ticket()
        {
            return View();
        }

        // EVENTS

        // Events listing page - shows all 6 events
        public IActionResult Events()
        {
            return View("~/Views/Events/Events.cshtml");
        }

        // Individual event pages
        public IActionResult Event1()
        {
            return View("~/Views/Events/Event1.cshtml");
        }

        public IActionResult Event2()
        {
            return View("~/Views/Events/Event2.cshtml");
        }

        public IActionResult Event3()
        {
            return View("~/Views/Events/Event3.cshtml");
        }

        public IActionResult Event4()
        {
            return View("~/Views/Events/Event4.cshtml");
        }

        public IActionResult Event5()
        {
            return View("~/Views/Events/Event5.cshtml");
        }

        public IActionResult Event6()
        {
            return View("~/Views/Events/Event6.cshtml");
        }

        // Event booking page
        public IActionResult EventBook()
        {
            return View("~/Views/Events/EventBook.cshtml");
        }

        // Event ticket page (shown after booking)
        public IActionResult EventTicket()
        {
            return View("~/Views/Events/EventTicket.cshtml");
        }
    }
}