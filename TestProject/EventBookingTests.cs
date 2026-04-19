using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Models;

namespace WebApplication1.Tests
{
    [TestClass]
    public class EventBookingTests
    {
        [TestMethod]
        public void EventBooking_Creation_ValidData_Success()
        {
            // Arrange
            var booking = new EventBooking
            {
                EventId = 1,
                TicketCode = "NG-ABCD-EFGH-IJKL",
                BookingDate = "2026-05-15",
                BookingTime = "14:00",
                Quantity = 3,
                Email = "test@example.com"
            };

            // Act & Assert
            Assert.AreEqual(1, booking.EventId);
            Assert.AreEqual(3, booking.Quantity);
            Assert.AreEqual("test@example.com", booking.Email);
        }

        [TestMethod]
        public void EventBooking_Quantity_WithinRange()
        {
            // Arrange
            var booking = new EventBooking { Quantity = 5 };

            // Act & Assert
            Assert.IsTrue(booking.Quantity >= 1 && booking.Quantity <= 5);
        }

        [TestMethod]
        public void EventBooking_Email_ValidFormat()
        {
            // Arrange
            var booking = new EventBooking { Email = "test@example.com" };

            // Act & Assert
            Assert.IsTrue(booking.Email.Contains("@"));
            Assert.IsTrue(booking.Email.Contains("."));
        }
    }
}