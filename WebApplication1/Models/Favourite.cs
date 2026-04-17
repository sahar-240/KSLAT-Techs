using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    // Tracks events or tours that a logged-in user has saved/hearted.
    // One of EventId or TourId will be set; the other stays null.
    public class Favourite
    {
        [Key]
        public int FavouriteId { get; set; }

        // The user who saved this item (requires login)
        public int UserId { get; set; }

        // Nullable foreign keys - only one is populated per row
        public int? EventId { get; set; }
        public int? TourId { get; set; }

        [ForeignKey("EventId")]
        public Event? Event { get; set; }

        [ForeignKey("TourId")]
        public Tour? Tour { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}