using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDCBank.Models
{
    public class Account
    {
        [Key]
        public int AccountID { get; set; }

        [Required]
        [MaxLength(20)]
        public string AccountType { get; set; }

        [Required(ErrorMessage = "Account number is required.")]
        [MaxLength(11)]
        public string AccountNumber { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        [Required]
        public DateTime OpenDate   { get; set; }
            

        // Foreign key
        [ForeignKey("CustomerID")]
        public int customer { get; set; }

        // Navigation property
        public Customer Customer { get; set; }

        // Navigation property
        public List<Transaction> Transactions { get; set; }
    }

}
