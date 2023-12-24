using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDCBank.Models
{
    public class UserCredential
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public int CustomerID   { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(30)]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(30)]
        public string SecurityQuestion { get; set; }

        [Required]
        [MaxLength(30)]
        public string SecurityAnswer    { get; set; }

        [ForeignKey("CustomerID")]
        public Customer Customer { get; set; }



    }
}
