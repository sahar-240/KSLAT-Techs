using WebApplication1.Models;

namespace WebApplication1.Data
{
    public static class DbSeeder
    {
        public static void Seed(MuseumDbContext db)
        {
            // Events seeding - COMMENTED OUT until team member merges their Events changes
            // The Azure Events table has a SpotsPerSlot column that our model doesn't have yet
            /*
            if (!db.Events.Any())
            {
                // ... events code ...
            }
            */

            if (!db.OpeningHours.Any())
            {
                var hours = new List<OpeningHour>
                {
                    new()
                    {
                        DayOfWeek = "Monday",
                        OpeningTime = TimeSpan.Parse("10:00"),
                        ClosingTime = TimeSpan.Parse("17:00"),
                        IsClosed = false
                    },
                    new()
                    {
                        DayOfWeek = "Tuesday",
                        OpeningTime = TimeSpan.Parse("10:00"),
                        ClosingTime = TimeSpan.Parse("17:00"),
                        IsClosed = false
                    },
                    new()
                    {
                        DayOfWeek = "Wednesday",
                        OpeningTime = TimeSpan.Parse("10:00"),
                        ClosingTime = TimeSpan.Parse("17:00"),
                        IsClosed = false
                    },
                    new()
                    {
                        DayOfWeek = "Thursday",
                        OpeningTime = TimeSpan.Parse("10:00"),
                        ClosingTime = TimeSpan.Parse("20:00"),
                        IsClosed = false,
                        Notes = "Extended hours"
                    },
                    new()
                    {
                        DayOfWeek = "Friday",
                        OpeningTime = TimeSpan.Parse("10:00"),
                        ClosingTime = TimeSpan.Parse("17:00"),
                        IsClosed = false
                    },
                    new()
                    {
                        DayOfWeek = "Saturday",
                        OpeningTime = TimeSpan.Parse("11:00"),
                        ClosingTime = TimeSpan.Parse("18:00"),
                        IsClosed = false
                    },
                    new()
                    {
                        DayOfWeek = "Sunday",
                        OpeningTime = TimeSpan.Parse("12:00"),
                        ClosingTime = TimeSpan.Parse("17:00"),
                        IsClosed = false
                    }
                };
                db.OpeningHours.AddRange(hours);
                db.SaveChanges();
            }

            // Seed a default user if none exist
            if (!db.Users.Any())
            {
                db.Users.Add(new User
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@museum.com",
                    Username = "admin",
                    PasswordHash = Convert.ToBase64String(
                        System.Security.Cryptography.SHA256.HashData(
                            System.Text.Encoding.UTF8.GetBytes("admin123")))
                });

                db.SaveChanges();
            }
        }
    }
}