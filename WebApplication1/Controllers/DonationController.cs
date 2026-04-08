using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public class DonationController(MuseumDbContext dbContext, ILogger<DonationController> logger, IPaymentService paymentService) : Controller
    {
        [HttpGet]
        public IActionResult Donation()
        {
            return View(new DonationViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Donation(DonationViewModel model)
        {
            try
            {
                // Log validation errors if any
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var error in errors)
                    {
                        logger.LogError($"Model validation error: {error.ErrorMessage}");
                    }
                }

                // Create donation - use null coalescing for safety
                var donation = new Donation
                {
                    FirstName = model.FirstName ?? string.Empty,
                    LastName = model.LastName ?? string.Empty,
                    Email = model.Email ?? string.Empty,
                    Phone = model.Phone ?? string.Empty,
                    Address = model.Address ?? string.Empty,
                    City = model.City ?? string.Empty,
                    Country = model.Country ?? string.Empty,
                    Postcode = model.Postcode ?? string.Empty,
                    Amount = model.Amount > 0 ? model.Amount : 0,
                    Message = model.Message ?? string.Empty,
                    SubscribeNewsletter = model.SubscribeNewsletter,  // Checkbox binding
                    Status = "Pending",
                    DonationDate = DateTime.Now
                };

                // Validate critical fields
                if (string.IsNullOrEmpty(donation.FirstName) ||
                    string.IsNullOrEmpty(donation.LastName) ||
                    string.IsNullOrEmpty(donation.Email) ||
                    donation.Amount <= 0)
                {
                    ModelState.AddModelError("", "Please fill in all required fields correctly.");
                    return View(model);
                }

                // Process payment
                var paymentResult = await paymentService.ProcessPaymentAsync(donation);

                if (!paymentResult.Success)
                {
                    ModelState.AddModelError("", paymentResult.Message);
                    return View(model);
                }

                // Update donation with transaction details
                donation.TransactionId = paymentResult.TransactionId;
                donation.Status = "Completed";

                // Save to database
                dbContext.Donations.Add(donation);
                await dbContext.SaveChangesAsync();

                logger.LogInformation($"Donation saved successfully. ID: {donation.Id}, Amount: £{donation.Amount}, Newsletter: {donation.SubscribeNewsletter}");

                return RedirectToAction("DonationConfirmation", new { id = donation.Id });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing donation");
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View(model);
            }
        }

        public async Task<IActionResult> DonationConfirmation(int id)
        {
            var donation = await dbContext.Donations.FindAsync(id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }
    }
}