using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("TourBooking")]
    public class TourBooking
    {
        [Key]
        public int TourBookingId { get; set; }

        public int TourId { get; set; }

        [ForeignKey("TourId")]
        public Tour? Tour { get; set; }

        public int? UserId { get; set; }

        [StringLength(30)]
        public string TicketCode { get; set; } = "";

        [StringLength(100)]
        public string BookingDate { get; set; } = "";

        [StringLength(20)]
        public string BookingTime { get; set; } = "";

        [Range(1, 10)]
        public int Quantity { get; set; } = 1;

        [Range(0.01, 10000)]
        public decimal Price { get; set; } = 25.00m;

        [Range(0.01, 100000)]
        public decimal TotalPrice { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } = "";

        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(100)]
        public string? CardholderName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}