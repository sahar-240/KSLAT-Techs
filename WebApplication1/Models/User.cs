using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    // Represents a registered museum member.
    // Stored in the Users table in Azure SQL.
    // UserId is the shared key used by EventBooking, Favourites, Tickets, and Donations.
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(100)]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100)]
        public string LastName { get; set; } = "";

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        [StringLength(255)]
        public string Email { get; set; } = "";

        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50)]
        public string Username { get; set; } = "";

        // Store hashed password — never store plain text
        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = "";

        [StringLength(10)]
        public string? Title { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? County { get; set; }

        [StringLength(20)]
        public string? Postcode { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}