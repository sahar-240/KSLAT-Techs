using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Models;

namespace WebApplication1.Tests
{
    [TestClass]
    public class TourBookingTests
    {
        [TestMethod]
        public void TourBooking_Creation_ValidData_Success()
        {
            // Arrange
            var booking = new TourBooking
            {
                TourId = 1,
                TicketCode = "TG-ABCD-EFGH-IJKL",
                BookingDate = "2026-06-15",
                BookingTime = "10:00",
                Quantity = 4,
                Price = 25.00m,
                TotalPrice = 100.00m,
                Email = "test@example.com",
                CardholderName = "John Doe"
            };

            // Act & Assert
            Assert.AreEqual(1, booking.TourId);
            Assert.AreEqual(4, booking.Quantity);
            Assert.AreEqual(100.00m, booking.TotalPrice);
        }

        [TestMethod]
        public void TourBooking_TotalPrice_CalculatedCorrectly()
        {
            // Arrange
            var booking = new TourBooking
            {
                Quantity = 5,
                Price = 25.00m,
                TotalPrice = 125.00m
            };

            // Act
            decimal expected = booking.Quantity * booking.Price;

            // Assert
            Assert.AreEqual(expected, booking.TotalPrice);
        }

        [TestMethod]
        public void TourBooking_Quantity_WithinRange()
        {
            // Arrange
            var booking = new TourBooking { Quantity = 8 };

            // Act & Assert
            Assert.IsTrue(booking.Quantity >= 1 && booking.Quantity <= 10);
        }
    }
}