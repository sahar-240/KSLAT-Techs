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
    public IActionResult Contact()
    {
        return View();
    }
    public IActionResult About() => View();
    public IActionResult Terms() => View();
    public IActionResult Support()
    {
        return View();
    }
    public IActionResult Donation()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ProcessDonation(string firstName, string lastName, string email,
        string phone, string address, string city, string country, string postcode,
        decimal amount, string comment)
    {
        // Handle payment processing here
        // Integrate with payment gateway (Stripe, PayPal, etc.)

        return RedirectToAction("DonationConfirmation");
    }

    public IActionResult DonationConfirmation()
    {
        return View();
    }
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}