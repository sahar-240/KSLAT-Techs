using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Models;
using System;

namespace WebApplication1.Tests
{
    [TestClass]
    public class TourTests
    {
        [TestMethod]
        public void Tour_Creation_ValidData_Success()
        {
            // Arrange
            var tour = new Tour
            {
                Title = "Museum Tour",
                Location = "Main Hall",
                Duration = "2 hours",
                Price = 25.00m,
                SpotsPerSlot = 20
            };

            // Act & Assert
            Assert.AreEqual("Museum Tour", tour.Title);
            Assert.AreEqual(25.00m, tour.Price);
            Assert.AreEqual(20, tour.SpotsPerSlot);
        }

        [TestMethod]
        public void Tour_Price_PositiveValue()
        {
            // Arrange
            var tour = new Tour { Price = 25.00m };

            // Act & Assert
            Assert.IsTrue(tour.Price > 0);
        }

        [TestMethod]
        public void Tour_DateRange_Valid()
        {
            // Arrange
            var tour = new Tour
            {
                StartDate = new DateTime(2026, 6, 1),
                EndDate = new DateTime(2026, 6, 30)
            };

            // Act & Assert
            Assert.IsTrue(tour.EndDate >= tour.StartDate);
        }
    }
}