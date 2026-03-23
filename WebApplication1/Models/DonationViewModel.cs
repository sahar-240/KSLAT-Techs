using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class DonationViewModel
    {
        public int Id { get; set; }
        public string DonorName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public bool IsAnonymous { get; set; }
        public bool GiftAid { get; set; }
        public bool SubscribeNewsletter { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime DonationDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending";

        // Address fields
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Postcode { get; set; } = string.Empty;

        // Card fields
        public string CardholderName { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string Expiry { get; set; } = string.Empty;
        public string CVV { get; set; } = string.Empty;
    }
}