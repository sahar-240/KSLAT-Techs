using WebApplication1.Models;

namespace WebApplication1.Data
{
    // Seeds the database with initial data when the tables are empty.
    // This runs once on first startup.
    // The "if Any()" checks prevent duplicate rows from being inserted on subsequent launches.
    public static class DbSeeder
    {
        public static void Seed(MuseumDbContext db)
        {
            // -- Seed events only if the table is empty --
            if (!db.Events.Any())
            {
                var events = new List<Event>
                {
                    new Event
                    {
                        Title = "Iris Van Herpen",
                        EventType = "Exhibition",
                        Genre = "Fashion",
                        Description = "Dutch fashion designer Iris Van Herpen merges couture with nature through biomimetic inspiration. Her work explores the intersection of organic forms and innovative technology.",
                        FullDescription = "Dutch fashion designer Iris Van Herpen merges couture with nature through biomimetic inspiration. Her work explores the intersection of organic forms and innovative technology, pushing the boundaries of what fashion can be.",
                        ImagePath = "~/images/Picture11.png",
                        Location = "Elizabeth Gallery, Floor 1",
                        StartDate = new DateTime(2026, 4, 16),
                        EndDate = new DateTime(2026, 5, 31),
                        TimeInfo = "Mon-Fri 9:00 AM - 5:00 PM, Sat 10:00 AM - 6:00 PM",
                        ThemeColour = "#B6827C",
                        IsFreeEntry = true,
                        SpotsPerSlot = 15
                    },
                    new Event
                    {
                        Title = "One Earth",
                        EventType = "Exhibition",
                        Genre = "Photography",
                        Description = "This photography exhibition documents the effects of global warming across the world's most vulnerable landscapes, capturing the impact of climate change.",
                        FullDescription = "This photography exhibition documents the effects of global warming across the world's most vulnerable landscapes. Featuring work by environmental photographers from around the globe.",
                        ImagePath = "~/images/Picture12.png",
                        Location = "Churchill Wing, Floor 2",
                        StartDate = new DateTime(2026, 7, 7),
                        EndDate = new DateTime(2026, 9, 15),
                        TimeInfo = "Mon-Fri 9:00 AM - 5:00 PM, Sat 10:00 AM - 6:00 PM",
                        ThemeColour = "#38ADC6",
                        IsFreeEntry = true,
                        SpotsPerSlot = 15
                    },
                    new Event
                    {
                        Title = "Daniel Schuller",
                        EventType = "Exhibition",
                        Genre = "Paintings/Drawings",
                        Description = "Born 1731, Swedish botanist Daniel Schuller was a student of Carl Linnaeus who spent decades cataloguing and naming over 1000 plant species.",
                        FullDescription = "Born 1731, Swedish botanist Daniel Schuller was a student of Carl Linnaeus who spent decades cataloguing and naming over 1000 plant species. His detailed botanical illustrations remain some of the most accurate records of European flora.",
                        ImagePath = "~/images/Picture9.png",
                        Location = "Napoleon Wing, Ground Floor",
                        StartDate = new DateTime(2026, 4, 15),
                        EndDate = new DateTime(2026, 6, 30),
                        TimeInfo = "Mon-Fri 10:00 AM - 4:00 PM, Sat 10:00 AM - 5:00 PM",
                        ThemeColour = "#9EAEBE",
                        IsFreeEntry = true,
                        SpotsPerSlot = 15
                    },
                    new Event
                    {
                        Title = "Thierry Mugler",
                        EventType = "Exhibition",
                        Genre = "Fashion",
                        Description = "French fashion designer Thierry Mugler is celebrated for bold futuristic design and nature-inspired silhouettes, featuring sculptural animal-inspired forms.",
                        FullDescription = "French fashion designer Thierry Mugler is celebrated for bold futuristic design and nature-inspired silhouettes. His iconic collections feature sculptural animal-inspired forms that blur the line between fashion and art.",
                        ImagePath = "~/images/Picture13.png",
                        Location = "Augustus Room, Floor 3",
                        StartDate = new DateTime(2026, 6, 22),
                        EndDate = new DateTime(2026, 8, 4),
                        TimeInfo = "Mon-Fri 11:00 AM - 5:00 PM, Sat 11:00 AM - 6:00 PM",
                        ThemeColour = "#8CB16C",
                        IsFreeEntry = true,
                        SpotsPerSlot = 15
                    },
                    new Event
                    {
                        Title = "Craft Mania",
                        EventType = "Workshop",
                        Genre = "Workshop",
                        Description = "A fun hands-on workshop where children learn to care for the planet through upcycled crafts. Discover how simple eco-friendly actions can make a big difference.",
                        FullDescription = "A fun hands-on workshop where children learn to care for the planet through upcycled crafts and activities. Discover how simple eco-friendly actions can make a big difference to the environment.",
                        ImagePath = "~/images/Picture14.png",
                        Location = "Victoria Plaza, 26 - Floor 2",
                        StartDate = new DateTime(2026, 4, 12),
                        EndDate = new DateTime(2026, 4, 14),
                        TimeInfo = "Mon-Fri 2:00 PM - 5:00 PM",
                        ThemeColour = "#D45D40",
                        IsFreeEntry = true,
                        SpotsPerSlot = 15
                    },
                    new Event
                    {
                        Title = "Extinction Event",
                        EventType = "Exhibition",
                        Genre = "Paintings/Drawings",
                        Description = "A series of paintings and drawings documenting the passenger pigeon, once a flourishing species across the Americas.",
                        FullDescription = "A series of paintings and drawings documenting the passenger pigeon, once a flourishing species across the Americas. This exhibition explores what we have lost and what remains at risk.",
                        ImagePath = "~/images/Picture15.png",
                        Location = "Marco Polo Hall, All Floors",
                        StartDate = new DateTime(2026, 8, 5),
                        EndDate = new DateTime(2026, 10, 20),
                        TimeInfo = "Mon-Fri 9:00 AM - 5:00 PM, Sat 10:00 AM - 6:00 PM",
                        ThemeColour = "#8C6A51",
                        IsFreeEntry = true,
                        SpotsPerSlot = 15
                    }
                };

                db.Events.AddRange(events);
                db.SaveChanges();
            }

            // -- Seed opening hours only if the table is empty --
            if (!db.OpeningHours.Any())
            {
                var hours = new List<OpeningHour>
                {
                    new() { DayOfWeek = "Monday",    OpeningTime = TimeSpan.Parse("10:00"), ClosingTime = TimeSpan.Parse("17:00"), IsClosed = false },
                    new() { DayOfWeek = "Tuesday",   OpeningTime = TimeSpan.Parse("10:00"), ClosingTime = TimeSpan.Parse("17:00"), IsClosed = false },
                    new() { DayOfWeek = "Wednesday", OpeningTime = TimeSpan.Parse("10:00"), ClosingTime = TimeSpan.Parse("17:00"), IsClosed = false },
                    new() { DayOfWeek = "Thursday",  OpeningTime = TimeSpan.Parse("10:00"), ClosingTime = TimeSpan.Parse("20:00"), IsClosed = false, Notes = "Extended hours" },
                    new() { DayOfWeek = "Friday",    OpeningTime = TimeSpan.Parse("10:00"), ClosingTime = TimeSpan.Parse("17:00"), IsClosed = false },
                    new() { DayOfWeek = "Saturday",  OpeningTime = TimeSpan.Parse("11:00"), ClosingTime = TimeSpan.Parse("18:00"), IsClosed = false },
                    new() { DayOfWeek = "Sunday",    OpeningTime = TimeSpan.Parse("12:00"), ClosingTime = TimeSpan.Parse("17:00"), IsClosed = false }
                };

                db.OpeningHours.AddRange(hours);
                db.SaveChanges();
            }
        }
    }
}