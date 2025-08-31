using System.ComponentModel.DataAnnotations;

namespace TaskManagerApp.DTO
{
    public class UserUpdateDTO
    {

        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters.")]
        public string? Username { get; set; }

        [EmailAddress(ErrorMessage = "Email must be a valid email address.")]
        [StringLength(50, ErrorMessage = "Email cannot be longer than 50 characters.")]
        public string? Email { get; set; }

        [StringLength(30, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 30 characters.")]
        public string? FirstName { get; set; }

        [StringLength(30, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 30 characters.")]
        public string? LastName { get; set; }

        [StringLength(20, MinimumLength = 3, ErrorMessage = "Role must be between 3 and 20 characters.")]
        public string? Role { get; set; }

        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 50 characters.")]
        public string? Password { get; set; }
    }
}
