using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    // Central database context that registers all entity tables with the Entity Framework Core.
    // Used by controllers to read from and write to the Azure SQL database.
    public class MuseumDbContext : DbContext
    {
        public MuseumDbContext(DbContextOptions<MuseumDbContext> options) : base(options)
        {
        }

        // -- Table registrations --
        public DbSet<Event> Events { get; set; }
        public DbSet<EventBooking> EventBookings { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<OpeningHour> OpeningHours { get; set; }
        public DbSet<FAQ> FAQs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // FAQ rows get a default timestamp on insert
            modelBuilder.Entity<FAQ>()
                .Property(f => f.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            // Each EventBooking belongs to one Event (cascade delete)
            modelBuilder.Entity<EventBooking>()
                .HasOne(b => b.Event)
                .WithMany()
                .HasForeignKey(b => b.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // Each Favourite optionally links to one Event
            modelBuilder.Entity<Favourite>()
                .HasOne(f => f.Event)
                .WithMany()
                .HasForeignKey(f => f.EventId)
                .OnDelete(DeleteBehavior.SetNull);

            // Each Ticket optionally links to one EventBooking
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.EventBooking)
                .WithMany()
                .HasForeignKey(t => t.EventBookingId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}