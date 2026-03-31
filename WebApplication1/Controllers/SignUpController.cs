using Microsoft.AspNetCore.Mvc;

namespace YourProjectName.Controllers
{
    public class SignUpController : Controller
    {
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(IFormCollection form)
        {
            // Get form values
            var title = form["Title"];
            var firstName = form["FirstName"];
            var lastName = form["LastName"];
            var email = form["Email"];
            var phone = form["Phone"];
            var username = form["Username"];
            var address = form["Address"];
            var city = form["City"];
            var county = form["County"];
            var postcode = form["Postcode"];

            // TODO: Save to database here
            // For now, just return the view
            return View();
        }
    }
}