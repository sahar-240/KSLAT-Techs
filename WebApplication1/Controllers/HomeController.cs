using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    // IDE0290: Use primary constructor
    public class HomeController(ILogger<HomeController> logger, IMuseumData data) : Controller
    {
        // IDE0305: Collection initialisation simplified (ToList() -> [..])
        public IActionResult Index()
        {
            var viewModel = new HomeViewModel
            {
                OpeningHours = [.. data.GetOpeningHours()],
                FAQs = [.. data.GetActiveFaqs()]
            };
            return View(viewModel);
        }

        public IActionResult Privacy() => View();
        public IActionResult Visit() => View();
        public IActionResult Contact() => View();
        public IActionResult About() => View();
        public IActionResult Terms() => View();
        public IActionResult Support() => View();
        public IActionResult Donation() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProcessDonation(DonationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Save donation to database
                TempData["SuccessMessage"] = "Thank you for your donation!";
                return RedirectToAction("Donation");
            }
            return View("Donation", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
