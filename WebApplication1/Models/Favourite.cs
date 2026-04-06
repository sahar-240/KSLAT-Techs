using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Favourite
    {
        [Key]
        public int FavouriteId { get; set; }

        // Must be logged in to save favourites
        public int UserId { get; set; }

        // One of these will be set, the other null
        public int? EventId { get; set; }
        public int? TourId { get; set; }

        [ForeignKey("EventId")]
        public Event? Event { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}