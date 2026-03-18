using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class WhatsOnController : Controller
    {
        // GET: WhatsOn/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: WhatsOn/Events
        public IActionResult Events()
        {
            return View();
        }

        // GET: WhatsOn/Tours
        public IActionResult Tours()
        {
            return View();
        }
    }
}
