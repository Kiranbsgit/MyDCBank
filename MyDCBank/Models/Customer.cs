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
     
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }


        [MaxLength(225)]
        public string Address { get; set; }


        //foreign key 
        [Required]
        [ForeignKey("UserID")]
        public int UserID { get; set; }

        //navigation property to show relation between customer and user.
        
        public User User { get; set; }

        // to represent one to many relationship between  account and customer. ( one customer can have many cards).

        public List<Account> accounts { get; set; } = new List<Account>();


        
              // to represent one to many relationship between  customer and card. ( one customer can have many cards).
       public List<Card> cards { get; set; }
    }

    // to represent one to many relationship between  customer and card. ( one customer can have many cards).
   
    
}
