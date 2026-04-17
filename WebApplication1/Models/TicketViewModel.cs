namespace WebApplication1.Models
{
    public class TicketViewModel
    {
        public int Id { get; set; }               // EventBookingId or TourBookingId
        public string Title { get; set; } = "";   // Event/Tour Name/Title
        public string Description { get; set; } = "";
        public string Date { get; set; } = "";    // Booking Date
        public string Status { get; set; } = "";  // Could be set to "Paid" or whatever status
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }   // <-- add for total
        public string TicketCode { get; set; } = "";
        public string BookingTime { get; set; } = ""; // Add if you want to display time
        public string? Email { get; set; }
        public string? CardholderName { get; set; }
    }
}