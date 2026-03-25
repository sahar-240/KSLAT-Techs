using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    // "bridge" between C# code and the database
    public class MuseumDbContext : DbContext
    {
        public MuseumDbContext(DbContextOptions<MuseumDbContext> options) : base(options)
        {
        }

        // tables
        public DbSet<OpeningHour> OpeningHours { get; set; }
        public DbSet<FAQ> FAQs { get; set; }

        // Events tables
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Existing config for FAQs
            modelBuilder.Entity<FAQ>()
                .Property(f => f.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            // Event seeds will be added here later once SQL is connected
        }
    }
}