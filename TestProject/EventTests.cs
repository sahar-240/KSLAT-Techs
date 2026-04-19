using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Models;
using System;

namespace WebApplication1.Tests
{
    [TestClass]
    public class EventTests
    {
        [TestMethod]
        public void Event_Creation_ValidData_Success()
        {
            // Arrange
            var startDate = new DateTime(2026, 4, 16);
            var endDate = new DateTime(2026, 5, 31);
            var ev = new Event
            {
                Title = "Test Exhibition",
                EventType = "Exhibition",
                Genre = "Fashion",
                Description = "Test Description",
                Location = "Floor 1",
                StartDate = startDate,
                EndDate = endDate,
                SpotsPerSlot = 15,
                IsFreeEntry = true
            };

            // Act & Assert
            Assert.AreEqual("Test Exhibition", ev.Title);
            Assert.AreEqual(15, ev.SpotsPerSlot);
            Assert.IsTrue(ev.IsFreeEntry);
        }

        [TestMethod]
        public void Event_DateRange_EndAfterStart()
        {
            // Arrange
            var startDate = new DateTime(2026, 4, 16);
            var endDate = new DateTime(2026, 5, 31);
            var ev = new Event { StartDate = startDate, EndDate = endDate };

            // Act & Assert
            Assert.IsTrue(ev.EndDate > ev.StartDate);
        }

        [TestMethod]
        public void Event_SpotsPerSlot_ValidRange()
        {
            // Arrange
            var ev = new Event { SpotsPerSlot = 15 };

            // Act & Assert
            Assert.IsTrue(ev.SpotsPerSlot >= 1 && ev.SpotsPerSlot <= 100);
        }
    }
}