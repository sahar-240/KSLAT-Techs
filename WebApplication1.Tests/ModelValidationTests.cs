using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;
using Xunit;

namespace WebApplication1.Tests
{
    /// <summary>
    /// Tests the data annotation validation on model classes.
    /// Verifies that [Required], [StringLength], [Range], and [EmailAddress] attributes work correctly.
    /// These tests do not need a database — they test the models directly.
    /// </summary>
    public class ModelValidationTests
    {
        // Helper: runs all data annotation validators on a model
        private List<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);
            Validator.TryValidateObject(model, context, results, validateAllProperties: true);
            return results;
        }

        /// <summary>
        /// Test 1: An Event with all required fields should pass validation.
        /// </summary>
        [Fact]
        public void Event_ValidData_PassesValidation()
        {
            var ev = new Event
            {
                Title = "Test Exhibition",
                EventType = "Exhibition",
                Genre = "Fashion",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                SpotsPerSlot = 15
            };

            var results = ValidateModel(ev);
            Assert.Empty(results);
        }

        /// <summary>
        /// Test 2: An Event with no title should fail validation.
        /// </summary>
        [Fact]
        public void Event_MissingTitle_FailsValidation()
        {
            var ev = new Event
            {
                Title = "",  // Required field left empty
                SpotsPerSlot = 15
            };

            var results = ValidateModel(ev);
            Assert.Contains(results, r => r.MemberNames.Contains("Title"));
        }

        /// <summary>
        /// Test 3: An Event with SpotsPerSlot outside the 1-100 range should fail.
        /// </summary>
        [Fact]
        public void Event_SpotsOutOfRange_FailsValidation()
        {
            var ev = new Event
            {
                Title = "Test",
                SpotsPerSlot = 200  // Max is 100
            };

            var results = ValidateModel(ev);
            Assert.Contains(results, r => r.MemberNames.Contains("SpotsPerSlot"));
        }

        /// <summary>
        /// Test 4: An EventBooking with quantity outside 1-5 should fail.
        /// </summary>
        [Fact]
        public void EventBooking_QuantityOutOfRange_FailsValidation()
        {
            var booking = new EventBooking
            {
                Email = "test@email.com",
                Quantity = 10  // Max is 5
            };

            var results = ValidateModel(booking);
            Assert.Contains(results, r => r.MemberNames.Contains("Quantity"));
        }

        /// <summary>
        /// Test 5: An EventBooking with no email should fail validation.
        /// </summary>
        [Fact]
        public void EventBooking_MissingEmail_FailsValidation()
        {
            var booking = new EventBooking
            {
                Email = "",  // Required
                Quantity = 1
            };

            var results = ValidateModel(booking);
            Assert.Contains(results, r => r.MemberNames.Contains("Email"));
        }

        /// <summary>
        /// Test 6: A Contact with valid data should pass validation.
        /// </summary>
        [Fact]
        public void Contact_ValidData_PassesValidation()
        {
            var contact = new Contact
            {
                Name = "Jane Smith",
                Email = "jane@email.com",
                Subject = "Test",
                Message = "Hello there"
            };

            var results = ValidateModel(contact);
            Assert.Empty(results);
        }

        /// <summary>
        /// Test 7: A Contact with an invalid email format should fail.
        /// </summary>
        [Fact]
        public void Contact_InvalidEmail_FailsValidation()
        {
            var contact = new Contact
            {
                Name = "Jane",
                Email = "not-an-email",  // Invalid format
                Subject = "Test",
                Message = "Hello"
            };

            var results = ValidateModel(contact);
            Assert.Contains(results, r => r.MemberNames.Contains("Email"));
        }

        /// <summary>
        /// Test 8: A Contact with missing required fields should fail.
        /// </summary>
        [Fact]
        public void Contact_MissingRequired_FailsValidation()
        {
            var contact = new Contact
            {
                Name = "",     // Required
                Email = "",    // Required
                Subject = "",  // Required
                Message = ""   // Required
            };

            var results = ValidateModel(contact);
            Assert.True(results.Count >= 4); // At least 4 required fields missing
        }

        /// <summary>
        /// Test 9: An EventBooking with valid data should pass validation.
        /// </summary>
        [Fact]
        public void EventBooking_ValidData_PassesValidation()
        {
            var booking = new EventBooking
            {
                Email = "test@email.com",
                Quantity = 3,
                TicketCode = "NG-ABCD-EFGH-IJKL",
                BookingDate = "Mon, 4 May 2026",
                BookingTime = "10:00 AM"
            };

            var results = ValidateModel(booking);
            Assert.Empty(results);
        }
    }
}