using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Data;
using WebApplication1.Models;

public class HomeController(ILogger<HomeController> logger, IMuseumData data) : Controller
{
    public IActionResult Index()
    {
        logger.LogInformation("Home page accessed");
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
            try
            {
                logger.LogInformation("Donation processed successfully");
                TempData["SuccessMessage"] = "Thank you for your donation!";
                return RedirectToAction("Donation");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing donation");
                ModelState.AddModelError("", "Error processing donation. Please try again.");
            }
        }
        return View("Donation", model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}