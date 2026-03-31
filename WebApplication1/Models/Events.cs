using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    // This class represents one row in the Events table
    // Each property becomes a column in the database
    public class Event
    {
        [Key] // This is the primary key (unique ID for each event)
        public int EventId { get; set; }

        [Required] // This column can't be empty
        public string Title { get; set; } = "";

        // "Exhibition" or "Workshop"
        public string EventType { get; set; } = "";

        // "Fashion", "Photography", "Paintings/Drawings", "Workshop"
        public string Genre { get; set; } = "";

        // Short description shown on the events card
        public string Description { get; set; } = "";

        // Longer description shown on the individual event page
        public string FullDescription { get; set; } = "";

        // Path to the image file like "~/images/Picture11.png"
        public string ImagePath { get; set; } = "";

        // Where in the museum, e.g. "Elizabeth Gallery, Floor 1"
        public string Location { get; set; } = "";

        // When the event starts and ends
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Opening times text, e.g. "Daily 10:00 AM - 6:00 PM"
        public string TimeInfo { get; set; } = "";

        // The accent colour for this event's card, e.g. "#B6827C"
        public string ThemeColour { get; set; } = "";

        // Is it free? (all yours are, but good to have)
        public bool IsFreeEntry { get; set; } = true;
    }
}
