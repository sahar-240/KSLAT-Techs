using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public class DonationController : Controller
    {
        private readonly MuseumDbContext _db;
        private readonly ILogger<DonationController> _logger;
        private readonly IPaymentService _paymentService;

        public DonationController(MuseumDbContext db, ILogger<DonationController> logger, IPaymentService paymentService)
        {
            _db = db;
            _logger = logger;
            _paymentService = paymentService;
        }

        [HttpGet]
        public IActionResult Donation()
        {
            return View(new Donation());
        }

        [HttpPost]
        public async Task<IActionResult> Donation(Donation model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var paymentResult = await _paymentService.ProcessPaymentAsync(model);

                if (!paymentResult.Success)
                {
                    ModelState.AddModelError("", paymentResult.Message);
                    return View(model);
                }

                model.TransactionId = paymentResult.TransactionId;
                model.Status = "Completed";
                model.DonationDate = DateTime.Now;

                _db.Donations.Add(model);
                await _db.SaveChangesAsync();

                return RedirectToAction("DonationConfirmation", new { id = model.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred. Please try again.");
                return View(model);
            }
        }

        public async Task<IActionResult> DonationConfirmation(int id)
        {
            var donation = await _db.Donations.FindAsync(id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }
    }
}