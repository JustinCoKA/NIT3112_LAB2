using System.ComponentModel.DataAnnotations;

namespace JustinLee_Lab2.Models
{
    // Login input is separate from the stored Identity account and password hash.
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter your administrator email address.")]
        [EmailAddress(ErrorMessage = "Enter a valid administrator email address.")]
        [StringLength(256, ErrorMessage = "The email address must not exceed 256 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter your administrator password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
