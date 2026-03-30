using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class AmenitiesController : Controller
    {
        // GET: Amenities/Index
        public IActionResult Index()
        {
            return View();
        }
    }
}