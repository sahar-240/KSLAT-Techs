using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }

        // Must be logged in to view tickets
        public int UserId { get; set; }

        // Links to either an event booking or a tour booking
        public int? EventBookingId { get; set; }
        public int? TourBookingId { get; set; }

        [ForeignKey("EventBookingId")]
        public EventBooking? EventBooking { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
