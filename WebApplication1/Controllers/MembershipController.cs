using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class MembershipController : Controller
    {
        // Membership page
        public IActionResult Index()
        {
            return View();
        }
    }
}