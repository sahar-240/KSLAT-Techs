using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Models;

namespace WebApplication1.Tests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void User_Creation_ValidData_Success()
        {
            // Arrange
            var user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Username = "johndoe",
                PasswordHash = "hash123"
            };

            // Act & Assert
            Assert.AreEqual("John", user.FirstName);
            Assert.AreEqual("Doe", user.LastName);
            Assert.AreEqual("john@example.com", user.Email);
        }

        [TestMethod]
        public void User_Properties_NotNull()
        {
            // Arrange
            var user = new User();

            // Act & Assert
            Assert.IsNotNull(user.FirstName);
            Assert.IsNotNull(user.LastName);
        }
    }
}