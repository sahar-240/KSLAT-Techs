using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    // Records a single event ticket booking.
    // One row is created each time a visitor completes the booking form.
    // The [Table] attribute maps this class to the "EventBooking" table in SQL.
    [Table("EventBooking")]
    public class EventBooking
    {
        [Key]
        public int EventBookingId { get; set; }

        // Foreign key linking this booking to the Events table
        public int EventId { get; set; }

        [ForeignKey("EventId")]
        public Event? Event { get; set; }

        // Nullable because guests can book without creating an account
        public int? UserId { get; set; }

        // Unique ticket code generated at booking time (format: NG-XXXX-XXXX-XXXX)
        [StringLength(30)]
        public string TicketCode { get; set; } = "";

        // The date and time slot the visitor selected on the booking form
        [StringLength(100)]
        public string BookingDate { get; set; } = "";

        [StringLength(20)]
        public string BookingTime { get; set; } = "";

        // Number of tickets purchased (capped at 5 per booking)
        [Range(1, 5, ErrorMessage = "Quantity must be between 1 and 5")]
        public int Quantity { get; set; } = 1;

        // Contact email for booking confirmation
        [Required(ErrorMessage = "Email address is required for booking confirmation")]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } = "";

        // Phone number is optional
        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        // Timestamp set automatically when the booking is saved
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
