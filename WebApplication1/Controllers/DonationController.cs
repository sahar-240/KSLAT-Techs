using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public class DonationController : Controller
    {
        private readonly MuseumDbContext _dbContext;
        private readonly ILogger<DonationController> _logger;
        private readonly IPaymentService _paymentService;

        public DonationController(MuseumDbContext dbContext, ILogger<DonationController> logger, IPaymentService paymentService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _paymentService = paymentService;
        }

        [HttpGet]
        public IActionResult Donation()
        {
            return View(new DonationViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Donation(DonationViewModel model)
        {
            try
            {
                // Server-side validation of critical fields
                if (string.IsNullOrWhiteSpace(model.FirstName) ||
                    string.IsNullOrWhiteSpace(model.LastName) ||
                    string.IsNullOrWhiteSpace(model.Email) ||
                    string.IsNullOrWhiteSpace(model.Address) ||
                    string.IsNullOrWhiteSpace(model.City) ||
                    model.Amount <= 0)
                {
                    TempData["Error"] = "Please fill in all required fields and select an amount.";
                    return View(model);
                }

                // Map ViewModel to Donation entity
                var donation = new Donation
                {
                    FirstName = model.FirstName.Trim(),
                    LastName = model.LastName.Trim(),
                    Email = model.Email.Trim(),
                    Phone = model.Phone ?? string.Empty,
                    Address = model.Address.Trim(),
                    City = model.City.Trim(),
                    Country = model.Country ?? string.Empty,
                    Postcode = model.Postcode ?? string.Empty,
                    Amount = model.Amount,
                    Message = model.Message ?? string.Empty,
                    SubscribeNewsletter = model.SubscribeNewsletter,
                    Status = "Pending",
                    DonationDate = DateTime.Now
                };

                // Process payment through mock service
                var paymentResult = await _paymentService.ProcessPaymentAsync(donation);

                if (!paymentResult.Success)
                {
                    TempData["Error"] = paymentResult.Message;
                    return View(model);
                }

                // Update with transaction details and save
                donation.TransactionId = paymentResult.TransactionId;
                donation.Status = "Completed";

                _dbContext.Donations.Add(donation);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Donation saved. ID: {donation.Id}, Amount: £{donation.Amount}");

                return RedirectToAction("DonationConfirmation", new { id = donation.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing donation");
                TempData["Error"] = "An error occurred processing your donation. Please try again.";
                return View(model);
            }
        }

        public async Task<IActionResult> DonationConfirmation(int id)
        {
            var donation = await _dbContext.Donations.FindAsync(id);
            if (donation == null) return NotFound();
            return View(donation);
        }
    }
}