using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    // Stores messages submitted through the Contact Us page.
    // Each form submission creates one row in the Contacts table.
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(200)]
        public string Email { get; set; } = "";

        // Optional field, stored as null when left blank
        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(200)]
        public string Subject { get; set; } = "";

        [Required(ErrorMessage = "Message is required")]
        [StringLength(2000)]
        public string Message { get; set; } = "";

        // Department the enquiry is directed to (e.g. "Events & Bookings")
        [StringLength(100)]
        public string Department { get; set; } = "";

        // Tracks whether the user opted in to newsletter emails
        public bool SubscribeNewsletter { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}