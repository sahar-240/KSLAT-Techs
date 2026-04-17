using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    // Links a logged-in user to their booking records.
    // Displayed on the Membership > Tickets page so users can view all their bookings.
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }

        // The user who owns this ticket (requires login)
        public int UserId { get; set; }

        // Links to either an event booking or a tour booking (one will be null)
        public int? EventBookingId { get; set; }
        public int? TourBookingId { get; set; }

        [ForeignKey("EventBookingId")]
        public EventBooking? EventBooking { get; set; }

        [ForeignKey("TourBookingId")]
        public TourBooking? TourBooking { get; set;  }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
       
    }
}
