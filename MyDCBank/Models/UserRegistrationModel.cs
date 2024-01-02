using System.ComponentModel.DataAnnotations;
using Xunit;

namespace MyDCBank.Models
{
    public class UserRegistrationModel
    {
        [Key]
        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(50, ErrorMessage = "Username must be at most 50 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        [MaxLength(100, ErrorMessage = "Password must be at most 100 characters.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [MaxLength(100, ErrorMessage = "Email must be at most 100 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50, ErrorMessage = "First name must be at most 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(50, ErrorMessage = "Last name must be at most 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date of birth.")]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(225)]
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }


        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }


        [Required(ErrorMessage = "Phone number is required.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Security question is required.")]
        public string SecurityQuestion { get; set; }

        [Required(ErrorMessage = "Security answer is required.")]
        public string SecurityAnswer { get; set; }
    }
}
