using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDCBank.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionID { get; set; }

        [Required]
        public int AccountID { get; set; }

        [Required]
        [MaxLength(20)]
        public string TransactionType { get; set; }

        [Required]
        public decimal Amount   { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [ForeignKey("AccountID")]
        public Account Account { get; set; }
    }
}
