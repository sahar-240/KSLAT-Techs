namespace WebApplication1.Models
{
    // BUG FIX: Created missing ViewModel for Tickets.cshtml
    public class TicketViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string TicketCode { get; set; } = string.Empty;
    }
}