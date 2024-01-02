using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDCBank.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerID { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Birth date is required.")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }


        [MaxLength(225)]
        public string Address { get; set; }

        public List<Account>accounts { get; set; }

        [ForeignKey("UserID")]
        public int UserID { get; set; }

        //navigation property
        public User Users { get; set; }
    }
}
