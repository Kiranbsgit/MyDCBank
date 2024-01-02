using System.ComponentModel.DataAnnotations;

namespace MyDCBank.Models
{
    public class UserLoginModel
    {
        [Key]
        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(50, ErrorMessage = "Username must be at most 50 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(100, ErrorMessage = "Password must be at most 100 characters.")]
        public string Password { get; set; }
    }
}
