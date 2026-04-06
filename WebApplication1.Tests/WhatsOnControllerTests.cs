using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Data;
using WebApplication1.Models;
using Xunit;

namespace WebApplication1.Tests
{
    /// <summary>
    /// Tests the WhatsOnController covering events listing,
    /// event detail, booking with availability checks, and search.
    /// Uses an in-memory database so no real SQL connection is needed.
    /// </summary>
    public class WhatsOnControllerTests
    {
        // Helper: creates a fresh in-memory database seeded with test events
        private MuseumDbContext CreateSeededDb()
        {
            var options = new DbContextOptionsBuilder<MuseumDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var db = new MuseumDbContext(options);

            db.Events.AddRange(
                new Event
                {
                    EventId = 1,
                    Title = "Test Exhibition",
                    EventType = "Exhibition",
                    Genre = "Fashion",
                    Description = "A test exhibition",
                    FullDescription = "Full test description",
                    ImagePath = "~/images/test.png",
                    Location = "Gallery 1",
                    StartDate = new DateTime(2026, 5, 1),
                    EndDate = new DateTime(2026, 6, 30),
                    TimeInfo = "Mon-Fri 9:00 AM - 5:00 PM",
                    ThemeColour = "#B6827C",
                    IsFreeEntry = true,
                    SpotsPerSlot = 15
                },
                new Event
                {
                    EventId = 2,
                    Title = "Nature Workshop",
                    EventType = "Workshop",
                    Genre = "Workshop",
                    Description = "A fun workshop",
                    FullDescription = "Full workshop description",
                    ImagePath = "~/images/test2.png",
                    Location = "Room 2",
                    StartDate = new DateTime(2026, 7, 1),
                    EndDate = new DateTime(2026, 7, 15),
                    TimeInfo = "Mon-Fri 10:00 AM - 4:00 PM",
                    ThemeColour = "#38ADC6",
                    IsFreeEntry = true,
                    SpotsPerSlot = 10
                }
            );
            db.SaveChanges();
            return db;
        }

        private WhatsOnController CreateController(MuseumDbContext db)
        {
            var controller = new WhatsOnController(db);
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
            controller.TempData = new TempDataDictionary(httpContext, new Mock_TempDataProvider());
            return controller;
        }

        /// <summary>
        /// Test 1: Events listing should return all events from the database.
        /// </summary>
        [Fact]
        public async Task Events_ReturnsAllEvents()
        {
            var db = CreateSeededDb();
            var controller = CreateController(db);

            var result = await controller.Events();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Event>>(viewResult.Model);
            Assert.Equal(2, model.Count);
        }

        /// <summary>
        /// Test 2: EventDetail with a valid ID should return the correct event.
        /// </summary>
        [Fact]
        public async Task EventDetail_ValidId_ReturnsEvent()
        {
            var db = CreateSeededDb();
            var controller = CreateController(db);

            var result = await controller.EventDetail(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Event>(viewResult.Model);
            Assert.Equal("Test Exhibition", model.Title);
        }

        /// <summary>
        /// Test 3: EventDetail with a non-existent ID should return NotFound.
        /// </summary>
        [Fact]
        public async Task EventDetail_InvalidId_ReturnsNotFound()
        {
            var db = CreateSeededDb();
            var controller = CreateController(db);

            var result = await controller.EventDetail(999);

            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Test 4: Booking with invalid quantity (0 or 6+) should be rejected.
        /// </summary>
        [Fact]
        public async Task EventBook_InvalidQuantity_ReturnsError()
        {
            var db = CreateSeededDb();
            var controller = CreateController(db);

            var result = await controller.EventBook(
                1, "Mon, 4 May 2026", "10:00 AM",
                6, "test@email.com", null);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("EventBook", redirect.ActionName);
            Assert.Equal(0, await db.EventBookings.CountAsync());
        }

        /// <summary>
        /// Test 5: A valid booking should save to the database and generate a ticket code.
        /// </summary>
        [Fact]
        public async Task EventBook_ValidBooking_SavesAndRedirects()
        {
            var db = CreateSeededDb();
            var controller = CreateController(db);

            var result = await controller.EventBook(
                1, "Mon, 4 May 2026", "10:00 AM",
                2, "visitor@email.com", "07700900000");

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("EventTicket", redirect.ActionName);

            // Verify the booking was saved
            Assert.Equal(1, await db.EventBookings.CountAsync());
            var booking = await db.EventBookings.FirstAsync();
            Assert.Equal(2, booking.Quantity);
            Assert.Equal("visitor@email.com", booking.Email);
            Assert.StartsWith("NG-", booking.TicketCode);
        }

        /// <summary>
        /// Test 6: GetAvailableSpots should return correct remaining spots.
        /// </summary>
        [Fact]
        public async Task GetAvailableSpots_ReturnsCorrectCount()
        {
            var db = CreateSeededDb();
            // Pre-book 3 spots
            db.EventBookings.Add(new EventBooking
            {
                EventId = 1,
                TicketCode = "NG-TEST-TEST-TEST",
                BookingDate = "Mon, 4 May 2026",
                BookingTime = "10:00 AM",
                Quantity = 3,
                Email = "test@test.com"
            });
            db.SaveChanges();

            var controller = CreateController(db);

            var result = await controller.GetAvailableSpots(1, "Mon, 4 May 2026", "10:00 AM");

            var jsonResult = Assert.IsType<JsonResult>(result);
            // SpotsPerSlot is 15, 3 booked = 12 remaining
            var value = jsonResult.Value;
            var spots = value?.GetType().GetProperty("spots")?.GetValue(value);
            Assert.Equal(12, spots);
        }

        /// <summary>
        /// Test 7: Search should find events matching the query term.
        /// </summary>
        [Fact]
        public async Task SearchEvents_MatchingQuery_ReturnsResults()
        {
            var db = CreateSeededDb();
            var controller = CreateController(db);

            var result = await controller.SearchEvents("fashion");

            var jsonResult = Assert.IsType<JsonResult>(result);
            var items = jsonResult.Value as IEnumerable<object>;
            Assert.NotNull(items);
            Assert.Single(items); // Only "Test Exhibition" has Genre = Fashion
        }

        /// <summary>
        /// Test 8: Search with empty query should return empty list.
        /// </summary>
        [Fact]
        public async Task SearchEvents_EmptyQuery_ReturnsEmpty()
        {
            var db = CreateSeededDb();
            var controller = CreateController(db);

            var result = await controller.SearchEvents("");

            var jsonResult = Assert.IsType<JsonResult>(result);
            var items = jsonResult.Value as IEnumerable<object>;
            Assert.NotNull(items);
            Assert.Empty(items);
        }
    }
}