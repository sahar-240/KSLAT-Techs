using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("OpeningHours")]
    public class OpeningHour
    {
        [Key]
        [Column("HourId")]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string DayOfWeek { get; set; } = string.Empty;

        [Column("OpenTime")]
        public TimeSpan? OpeningTime { get; set; }

        [Column("CloseTime")]
        public TimeSpan? ClosingTime { get; set; }

        [Column("IsClosed")]
        public bool IsClosed { get; set; }

        [Column("SpecialNote")]
        [StringLength(255)]
        public string? Notes { get; set; }
    }
}