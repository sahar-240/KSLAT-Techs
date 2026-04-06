using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        public string Title { get; set; } = "";

        public string EventType { get; set; } = "";
        public string Genre { get; set; } = "";
        public string Description { get; set; } = "";
        public string FullDescription { get; set; } = "";
        public string ImagePath { get; set; } = "";
        public string Location { get; set; } = "";

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string TimeInfo { get; set; } = "";
        public string ThemeColour { get; set; } = "";
        public bool IsFreeEntry { get; set; } = true;

        // Available spots per time slot (e.g. 15)
        public int SpotsPerSlot { get; set; } = 15;
    }
}