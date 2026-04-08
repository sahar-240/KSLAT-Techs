using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    // Central database context — registers ALL team tables with Entity Framework Core.
    // EF uses these DbSet properties to create/manage SQL tables in Azure.
    public class MuseumDbContext : DbContext
    {
        public MuseumDbContext(DbContextOptions<MuseumDbContext> options) : base(options)
        {
        }

        // -- Louisa's tables --
        public DbSet<Event> Events { get; set; }
        public DbSet<EventBooking> EventBookings { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<OpeningHour> OpeningHours { get; set; }

        // -- Kai's table --
        public DbSet<User> Users { get; set; }

        // -- Tanzira's table --
        public DbSet<Donation> Donations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // -- Louisa's relationships --

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

            // -- Tanzira's Donation defaults --
            modelBuilder.Entity<Donation>()
                .Property(d => d.DonationDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Donation>()
                .Property(d => d.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Donation>()
                .Property(d => d.Status)
                .HasDefaultValue("Completed");

            // -- Kai's User unique index --
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}