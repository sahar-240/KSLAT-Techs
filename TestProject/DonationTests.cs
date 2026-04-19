using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Models;

namespace WebApplication1.Tests
{
    [TestClass]
    public class DonationTests
    {
        [TestMethod]
        public void Donation_Creation_ValidData_Success()
        {
            // Arrange
            var donation = new Donation
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com",
                Amount = 50.00m,
                City = "London",
                Address = "123 Main St",
                Status = "Completed"
            };

            // Act & Assert
            Assert.AreEqual("Jane", donation.FirstName);
            Assert.AreEqual(50.00m, donation.Amount);
            Assert.AreEqual("Completed", donation.Status);
        }

        [TestMethod]
        public void Donation_Amount_PositiveValue()
        {
            // Arrange
            var donation = new Donation { Amount = 25.00m };

            // Act & Assert
            Assert.IsTrue(donation.Amount > 0);
        }

        [TestMethod]
        public void Donation_Email_ValidFormat()
        {
            // Arrange
            var donation = new Donation { Email = "test@example.com" };

            // Act & Assert
            Assert.IsTrue(donation.Email.Contains("@"));
        }
    }
}