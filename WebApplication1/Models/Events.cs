using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    // Represents a single exhibition or workshop stored in the Events table.
    // Each row becomes one card on the Events listing page.
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        // Event title displayed on cards and detail pages
        [Required(ErrorMessage = "Event title is required")]
        [StringLength(200)]
        public string Title { get; set; } = "";

        // Category label (e.g. "Exhibition", "Workshop")
        [StringLength(100)]
        public string EventType { get; set; } = "";

        // Genre used for the filter dropdown (e.g. "Fashion", "Photography")
        [StringLength(100)]
        public string Genre { get; set; } = "";

        // Short summary shown on the Events listing cards
        public string Description { get; set; } = "";

        // Full description shown on the individual EventDetail page
        public string FullDescription { get; set; } = "";

        // Relative path to the event banner image (e.g. "~/images/Picture11.png")
        [StringLength(500)]
        public string ImagePath { get; set; } = "";

        // Physical location in the museum (e.g. "Elizabeth Gallery, Floor 1")
        [StringLength(200)]
        public string Location { get; set; } = "";

        // Date range the event runs between
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Opening hours text parsed by the booking page JavaScript
        // Format: "Mon-Fri 9:00 AM - 5:00 PM, Sat 10:00 AM - 6:00 PM"
        [StringLength(200)]
        public string TimeInfo { get; set; } = "";

        // Hex colour for the card border and detail page (e.g. "#B6827C")
        [StringLength(10)]
        public string ThemeColour { get; set; } = "";

        // Whether entry is free or requires payment
        public bool IsFreeEntry { get; set; } = true;

        // Maximum visitors allowed per hourly time slot
        [Range(1, 100)]
        public int SpotsPerSlot { get; set; } = 15;
    }
}