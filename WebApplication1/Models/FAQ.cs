using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    // FAQ data displayed on the homepage.
    // NOT stored in the database - loaded from InMemoryMuseumData instead.
    public class FAQ
    {
        [Key]
        public int FaqId { get; set; }

        public string Question { get; set; } = "";

        public string Answer { get; set; } = "";

        public string Category { get; set; } = "";

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}