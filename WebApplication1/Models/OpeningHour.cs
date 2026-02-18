using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class OpeningHour
    {
        [Key]
        public int HourId { get; set; }

        [Required]
        [MaxLength(20)]
        public string DayOfWeek { get; set; } = string.Empty;

        public TimeSpan? OpenTime { get; set; }

        public TimeSpan? CloseTime { get; set; }

        public bool IsClosed { get; set; } = false;

        [MaxLength(255)]
        public string? SpecialNote { get; set; }
    }
}