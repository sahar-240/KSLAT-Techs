using Microsoft.EntityFrameworkCore;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure default value for CreatedAt
            modelBuilder.Entity<FAQ>()
                .Property(f => f.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}