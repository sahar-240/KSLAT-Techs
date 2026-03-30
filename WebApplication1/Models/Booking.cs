using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    // This class represents one booking/ticket in the database
    // When someone books an event, a row is added here
    public class Booking
    {
        [Key] // Unique booking ID
        public int BookingId { get; set; }

        // Which event this booking is for (links to the Events table)
        public int EventId { get; set; }

        [ForeignKey("EventId")] // Tells EF this connects to the Event table
        public Event? Event { get; set; }

        // The unique ticket code like "NG-4F8A-K2M7-X9BL"
        public string TicketCode { get; set; } = "";

        // What date and time the user chose
        public string BookingDate { get; set; } = "";
        public string BookingTime { get; set; } = "";

        // How many tickets
        public int Quantity { get; set; } = 1;

        [Required] // Email is required
        public string Email { get; set; } = "";

        // Phone is optional (that's what the ? means - nullable)
        public string? Phone { get; set; }

        // When this booking was made
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
