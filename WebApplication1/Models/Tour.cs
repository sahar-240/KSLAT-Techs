using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Tour
    {
        [Key]
        public int TourId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = "";

        [StringLength(2000)]
        public string Description { get; set; } = "";

        [StringLength(5000)]
        public string FullDescription { get; set; } = "";

        [StringLength(500)]
        public string ImagePath { get; set; } = "";

        [StringLength(200)]
        public string Location { get; set; } = "";

        [StringLength(50)]
        public string Duration { get; set; } = "";

        [Range(0.01, 10000)]
        public decimal Price { get; set; } = 25.00m;

        [Range(1, 100)]
        public int SpotsPerSlot { get; set; } = 15;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [StringLength(200)]
        public string TimeInfo { get; set; } = "";

        [StringLength(10)]
        public string ThemeColour { get; set; } = "";

        [StringLength(100)]
        public string? Genre { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    }

    }