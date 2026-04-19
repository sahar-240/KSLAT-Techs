using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Models;

namespace WebApplication1.Tests
{
    [TestClass]
    public class ContactTests
    {
        [TestMethod]
        public void Contact_Creation_ValidData_Success()
        {
            // Arrange
            var contact = new Contact
            {
                Name = "John Doe",
                Email = "john@example.com",
                Subject = "Inquiry",
                Message = "Hello, I have a question",
                Department = "Events"
            };

            // Act & Assert
            Assert.AreEqual("John Doe", contact.Name);
            Assert.AreEqual("Events", contact.Department);
        }

        [TestMethod]
        public void Contact_Email_Contains_AtSymbol()
        {
            // Arrange
            var contact = new Contact { Email = "test@example.com" };

            // Act & Assert
            Assert.IsTrue(contact.Email.Contains("@"));
        }
    }
}