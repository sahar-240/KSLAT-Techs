using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class SignUpViewModel
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; } = "";

        [Required, MaxLength(100)]
        public string LastName { get; set; } = "";

        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; } = "";

        [Required, MinLength(4)]
        public string Password { get; set; } = "";
    }
}