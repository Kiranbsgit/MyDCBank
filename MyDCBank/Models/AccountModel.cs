using System.ComponentModel.DataAnnotations;

namespace MyDCBank.Models
{
    public class AccountModel
    {

        // this is of type enum --- savings, current, salary and government account types.
        [Required]
        public AccountType AccountType { get; set; }

        [Key]
        [Required]
        public int CustomerID { get; set; }
    }
    
}
