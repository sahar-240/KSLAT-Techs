using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class WhatsOnController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Tours()
        {
            return View();
        }

        public IActionResult Booking()
        {
            return View();
        }

        public IActionResult Ticket()
        {
            return View();
        }
    }
}