using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    // Handles the Contact Us form submission.
    // Validates input, saves to the Contacts table, and redirects to a success or error page.
    public class ContactController : Controller
    {
        private readonly MuseumDbContext _db;
        private readonly ILogger<ContactController> _logger;

        public ContactController(MuseumDbContext db, ILogger<ContactController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // POST: Receives form data, validates, and saves to the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(
            string Name, string Email, string? Phone,
            string Department, string Subject, string Message,
            bool SubscribeNewsletter = false)
        {
            try
            {
                // -- Server side input validation --
                if (string.IsNullOrWhiteSpace(Name) ||
                    string.IsNullOrWhiteSpace(Email) ||
                    string.IsNullOrWhiteSpace(Subject) ||
                    string.IsNullOrWhiteSpace(Message))
                {
                    TempData["ContactError"] = "Please fill in all required fields.";
                    return RedirectToAction("Contact", "Home");
                }

                if (!Email.Contains("@") || !Email.Contains("."))
                {
                    TempData["ContactError"] = "Please enter a valid email address.";
                    return RedirectToAction("Contact", "Home");
                }

                // Build the Contact entity and save
                var contact = new Contact
                {
                    Name = Name.Trim(),
                    Email = Email.Trim(),
                    Phone = string.IsNullOrWhiteSpace(Phone) ? null : Phone.Trim(),
                    Subject = Subject.Trim(),
                    Message = Message.Trim(),
                    Department = Department ?? "",
                    SubscribeNewsletter = SubscribeNewsletter,
                    CreatedAt = DateTime.UtcNow
                };

                _db.Contacts.Add(contact);
                await _db.SaveChangesAsync();

                return RedirectToAction("ContactSuccess", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save contact form submission");
                TempData["ContactError"] = "Something went wrong. Please try again.";
                return RedirectToAction("Contact", "Home");
            }
        }
    }
}
