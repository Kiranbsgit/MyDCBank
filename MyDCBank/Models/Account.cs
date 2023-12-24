using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDCBank.Models
{
    public class Account
    {
        [Key]
        public int AccountID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        [MaxLength(20)]
        public string AccountType { get; set; }

        [Required]
        public decimal Balance { get; set; }

        [Required]
        public DateTime OpenDate   { get; set; }

        public List<Transaction> Transactions { get; set; }

        [ForeignKey("CustomerID")]
        public Customer customer { get; set; }
    }
}
