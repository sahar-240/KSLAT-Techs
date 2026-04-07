using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    // Handles new user registration.
    // Validates input, checks for duplicate username/email,
    // hashes the password, and saves to the Users table.
    public class SignUpController : Controller
    {
        private readonly MuseumDbContext _db;

        public SignUpController(MuseumDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(
            string title, string firstName, string lastName, string email,
            string phone, string username, string password, string confirmPassword,
            string address, string city, string county, string postcode)
        {
            // -- Server-side validation --
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                TempData["ErrorMessage"] = "Please fill in all required fields.";
                return View();
            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                TempData["ErrorMessage"] = "Please enter a valid email address.";
                return View();
            }

            if (password.Length < 6)
            {
                TempData["ErrorMessage"] = "Password must be at least 6 characters.";
                return View();
            }

            if (password != confirmPassword)
            {
                TempData["ErrorMessage"] = "Passwords do not match.";
                return View();
            }

            // Check if username or email already exists
            if (await _db.Users.AnyAsync(u => u.Username == username))
            {
                TempData["ErrorMessage"] = "That username is already taken.";
                return View();
            }

            if (await _db.Users.AnyAsync(u => u.Email == email))
            {
                TempData["ErrorMessage"] = "An account with that email already exists.";
                return View();
            }

            // Hash the password (simple hash for coursework — production would use BCrypt)
            var hash = Convert.ToBase64String(
                System.Security.Cryptography.SHA256.HashData(
                    System.Text.Encoding.UTF8.GetBytes(password)));

            var user = new User
            {
                Title = title,
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                Email = email.Trim(),
                Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim(),
                Username = username.Trim(),
                PasswordHash = hash,
                Address = address,
                City = city,
                County = county,
                Postcode = postcode,
                CreatedAt = DateTime.UtcNow
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Welcome {firstName}! Sign up successful. Please log in.";
            return RedirectToAction("Index", "Login");
        }
    }
}