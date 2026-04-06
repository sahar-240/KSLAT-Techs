using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication1.Controllers;
using WebApplication1.Data;
using WebApplication1.Models;
using Xunit;

namespace WebApplication1.Tests
{
    /// <summary>
    /// Tests the ContactController to verify that contact form
    /// submissions are validated and saved correctly.
    /// Uses an in-memory database so no real SQL connection is needed.
    /// </summary>
    public class ContactControllerTests
    {
        // Helper: creates a fresh in-memory database for each test
        private MuseumDbContext CreateTestDb()
        {
            var options = new DbContextOptionsBuilder<MuseumDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new MuseumDbContext(options);
        }

        // Helper: creates a controller with TempData support
        private ContactController CreateController(MuseumDbContext db)
        {
            var logger = LoggerFactory.Create(b => { }).CreateLogger<ContactController>();
            var controller = new ContactController(db, logger);

            // TempData requires an HTTP context
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            controller.TempData = new TempDataDictionary(httpContext,
                new SessionStateTempDataProvider(new SessionStateTempDataProviderOptions()));

            return controller;
        }

        // Helper: creates TempData using a simple provider
        private TempDataDictionary CreateTempData(HttpContext httpContext)
        {
            var tempDataProvider = new CookieTempDataProvider(
                Microsoft.AspNetCore.DataProtection.DataProtectionProvider.Create("test"));
            return new TempDataDictionary(httpContext, tempDataProvider);
        }

        /// <summary>
        /// Test 1: A valid contact submission should save to the database
        /// and redirect to the ContactSuccess page.
        /// </summary>
        [Fact]
        public async Task SendMessage_ValidInput_SavesAndRedirects()
        {
            // Arrange
            var db = CreateTestDb();
            var logger = LoggerFactory.Create(b => { }).CreateLogger<ContactController>();
            var controller = new ContactController(db, logger);
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
            controller.TempData = new TempDataDictionary(httpContext,
                new Mock_TempDataProvider());

            // Act
            var result = await controller.SendMessage(
                "Jane Smith", "jane@email.com", "07700900000",
                "Events & Bookings", "Booking Question",
                "When is the next exhibition?", false);

            // Assert - should redirect to ContactSuccess
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ContactSuccess", redirect.ActionName);
            Assert.Equal("Home", redirect.ControllerName);

            // Assert - one row should exist in the Contacts table
            Assert.Equal(1, await db.Contacts.CountAsync());

            // Assert - the saved data should match what we submitted
            var saved = await db.Contacts.FirstAsync();
            Assert.Equal("Jane Smith", saved.Name);
            Assert.Equal("jane@email.com", saved.Email);
            Assert.Equal("Booking Question", saved.Subject);
        }

        /// <summary>
        /// Test 2: A submission with empty required fields should NOT save
        /// and should redirect back with an error message.
        /// </summary>
        [Fact]
        public async Task SendMessage_MissingName_ReturnsError()
        {
            // Arrange
            var db = CreateTestDb();
            var logger = LoggerFactory.Create(b => { }).CreateLogger<ContactController>();
            var controller = new ContactController(db, logger);
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
            controller.TempData = new TempDataDictionary(httpContext,
                new Mock_TempDataProvider());

            // Act - send with empty name
            var result = await controller.SendMessage(
                "", "jane@email.com", null,
                "General", "Test", "Hello", false);

            // Assert - should redirect back to Contact (not ContactSuccess)
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Contact", redirect.ActionName);

            // Assert - nothing should be saved
            Assert.Equal(0, await db.Contacts.CountAsync());
        }

        /// <summary>
        /// Test 3: A submission with an invalid email should NOT save.
        /// </summary>
        [Fact]
        public async Task SendMessage_InvalidEmail_ReturnsError()
        {
            // Arrange
            var db = CreateTestDb();
            var logger = LoggerFactory.Create(b => { }).CreateLogger<ContactController>();
            var controller = new ContactController(db, logger);
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
            controller.TempData = new TempDataDictionary(httpContext,
                new Mock_TempDataProvider());

            // Act - send with invalid email (no dot after @)
            var result = await controller.SendMessage(
                "Jane", "janeemail", null,
                "General", "Test", "Hello", false);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Contact", redirect.ActionName);
            Assert.Equal(0, await db.Contacts.CountAsync());
        }
    }

    /// <summary>
    /// Minimal TempData provider for unit tests.
    /// Stores values in a simple dictionary without needing
    /// a real HTTP session or cookies.
    /// </summary>
    public class Mock_TempDataProvider : ITempDataProvider
    {
        private IDictionary<string, object?> _data = new Dictionary<string, object?>();

        public IDictionary<string, object?> LoadTempData(HttpContext context) => _data;

        public void SaveTempData(HttpContext context, IDictionary<string, object?> values)
        {
            _data = values;
        }
    }
}