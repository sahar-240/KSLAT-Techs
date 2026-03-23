using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMuseumData _data;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IMuseumData data)
        {
            _logger = logger;
            _data = data;
        }

        public IActionResult Index()
        {
            var viewModel = new HomeViewModel
            {
                OpeningHours = _data.GetOpeningHours().ToList(),
                FAQs = _data.GetActiveFaqs().ToList()
            };

            return View(viewModel);
        }

        // SUPPORT PAGE
        public IActionResult Support()
        {
            var model = new SupportViewModel
            {
                Title = "Support Us",
                Description = "Help us preserve and share our natural heritage"
            };
            return View(model);
        }

        // DONATION PAGE
        public IActionResult Donation()
        {
            var model = new DonationViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProcessDonation(DonationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Here you would typically:
                    // 1. Save the donation to the database
                    // 2. Process payment through a payment gateway (Stripe, PayPal, etc.)
                    // 3. Send confirmation email

                    _logger.LogInformation($"Donation received: £{model.Amount} from {model.FirstName} {model.LastName}");

                    // For now, just redirect to success page
                    TempData["SuccessMessage"] = $"Thank you for your donation of £{model.Amount}!";
                    return RedirectToAction("Success");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing donation");
                    ModelState.AddModelError("", "An error occurred processing your donation. Please try again.");
                }
            }

            return View("Donation", model);
        }

        public IActionResult Success()
        {
            return View();
        }

        // VISIT PAGE
        public IActionResult Visit()
        {
            return View();
        }

        // WHAT'S ON PAGE
        public IActionResult WhatsOn()
        {
            return View();
        }

        // CONTACT PAGE
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Here you would typically:
                    // 1. Save the contact form to database
                    // 2. Send email to support team
                    // 3. Send confirmation email to user

                    _logger.LogInformation($"Contact form received from {model.Name} ({model.Email})");

                    TempData["SuccessMessage"] = "Thank you for your message! We'll get back to you soon.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing contact form");
                    ModelState.AddModelError("", "An error occurred. Please try again.");
                }
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}