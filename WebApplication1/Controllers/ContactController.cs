using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ContactController : Controller
    {
        private readonly MuseumDbContext _db;
        private readonly ILogger<ContactController> _logger;

        public ContactController(MuseumDbContext db, ILogger<ContactController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(ContactViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var contact = new Contact
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Phone = model.Phone ?? "",
                        Subject = model.Subject,
                        Message = model.Message,
                        Department = model.Department,
                        SubscribeNewsletter = model.SubscribeNewsletter,
                        CreatedAt = DateTime.UtcNow
                    };

                    _db.Contacts.Add(contact);
                    await _db.SaveChangesAsync();

                    return RedirectToAction("ContactSuccess", "Home");
                }

                return RedirectToAction("Contact", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return RedirectToAction("Contact", "Home");
            }
        }
    }
}
