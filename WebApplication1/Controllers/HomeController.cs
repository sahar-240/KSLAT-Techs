using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController(ILogger<HomeController> logger, IMuseumData data, MuseumDbContext dbContext) : Controller
    {
        public IActionResult Index()
        {
            logger.LogInformation("Home page accessed");

            var openingHoursFromDb = dbContext.OpeningHours
                .ToList()
                .OrderBy(oh => GetDayOrder(oh.DayOfWeek))
                .ToList();

            var viewModel = new HomeViewModel
            {
                OpeningHours = openingHoursFromDb.Count > 0 ? openingHoursFromDb : [.. data.GetOpeningHours()],
                FAQs = [.. data.GetActiveFaqs()]
            };
            return View(viewModel);
        }

        public IActionResult Privacy() => View();
        public IActionResult Visit() => View();
        public IActionResult Contact() => View();
        public IActionResult ContactSuccess() => View();
        public IActionResult About() => View();
        public IActionResult Terms() => View();
        public IActionResult Support() => View();

        // Donations are now handled by DonationController (Tanzira)
        // This redirect keeps old links working
        public IActionResult Donation()
        {
            return RedirectToAction("Donation", "Donation");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private static int GetDayOrder(string day)
        {
            return day.ToLower() switch
            {
                "monday" => 0,
                "tuesday" => 1,
                "wednesday" => 2,
                "thursday" => 3,
                "friday" => 4,
                "saturday" => 5,
                "sunday" => 6,
                _ => 7
            };
        }
    }
}