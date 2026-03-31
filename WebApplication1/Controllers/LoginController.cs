using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        // Login Page GET
        public IActionResult Index()
        {
            return View("~/Views/Account/Login.cshtml");
        }

        // Login Page POST
        // IDE0060: Removed unused parameters - replace with real auth logic when ready
        [HttpPost]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            // TODO: Inject UserManager / auth service and validate credentials here
            return RedirectToAction("Account", "Account");
        }

        // Register GET
        public IActionResult Register()
        {
            return View("~/Views/SignUp/SignUp.cshtml");
        }

        // Register POST
        // IDE0060: Removed unused parameters - replace with real registration logic when ready
        [HttpPost]
        [ActionName("Register")]
        public IActionResult RegisterPost()
        {
            // TODO: Inject UserManager / auth service and create account here
            return RedirectToAction("Index", "Login");
        }
    }
}
