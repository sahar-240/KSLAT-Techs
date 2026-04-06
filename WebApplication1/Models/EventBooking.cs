using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("EventBooking")]
    public class EventBooking
    {
        [Key]
        public int EventBookingId { get; set; }

        // Which event this booking is for
        public int EventId { get; set; }

        [ForeignKey("EventId")]
        public Event? Event { get; set; }

        // Nullable – user doesn't need an account to book
        public int? UserId { get; set; }

        public string TicketCode { get; set; } = "";
        public string BookingDate { get; set; } = "";
        public string BookingTime { get; set; } = "";
        public int Quantity { get; set; } = 1;

        [Required]
        public string Email { get; set; } = "";
        public string? Phone { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
