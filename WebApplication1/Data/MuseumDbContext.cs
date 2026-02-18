using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class MuseumDbContext : DbContext
    {
        public MuseumDbContext(DbContextOptions<MuseumDbContext> options) : base(options)
        {
        }

        public DbSet<OpeningHour> OpeningHours { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        // Add other DbSets as needed
    }
}