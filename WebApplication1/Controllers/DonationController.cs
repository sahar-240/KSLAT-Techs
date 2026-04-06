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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Map ViewModel to Donation model FIRST
                var donation = new Donation
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Phone = model.Phone,
                    Address = model.Address,
                    City = model.City,
                    Country = model.Country,
                    Postcode = model.Postcode,
                    Amount = model.Amount,
                    Message = model.Message,
                    SubscribeNewsletter = model.SubscribeNewsletter,
                    Status = "Pending",
                    DonationDate = DateTime.Now
                };

                // Pass Donation model to payment service
                var paymentResult = await paymentService.ProcessPaymentAsync(donation);

                if (!paymentResult.Success)
                {
                    ModelState.AddModelError("", paymentResult.Message);
                    return View(model);
                }

                donation.TransactionId = paymentResult.TransactionId;
                donation.Status = "Completed";

                dbContext.Donations.Add(donation);
                await dbContext.SaveChangesAsync();

                return RedirectToAction("DonationConfirmation", new { id = donation.Id });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing donation");
                ModelState.AddModelError("", "An error occurred. Please try again.");
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