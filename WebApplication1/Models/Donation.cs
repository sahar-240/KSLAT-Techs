using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Donation
    {
        [Key]
        [Column("DonationId")]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "DECIMAL(10,2)")]
        public decimal Amount { get; set; }

        [StringLength(500)]
        public string Message { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [StringLength(100)]
        public string Country { get; set; } = string.Empty;

        [StringLength(20)]
        public string Postcode { get; set; } = string.Empty;

        public bool SubscribeNewsletter { get; set; } = false;

        [Required]
        public DateTime DonationDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Completed";

        [StringLength(int.MaxValue)]
        public string TransactionId { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}