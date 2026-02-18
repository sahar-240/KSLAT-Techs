using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class FAQ
    {
        [Key]
        public int FaqId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Question { get; set; } = string.Empty;

        [Required]
        public string Answer { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Category { get; set; }

        public int DisplayOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}