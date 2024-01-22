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
        [MaxLength(20)]
        public TransactionType TransactionType { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount   { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }
       
        public int AccountID { get; set; }
        // Navigation property
        [ForeignKey("AccountID")]
        public Account Account { get; set; }

        // to incorporate transaction done by cards.
        public int? CardID { get; set; }

        [ForeignKey("CardID")]
        public Card Card { get; set; }


    }

    public enum TransactionType
    {
        UPI,
        Debit,
        Credit,
        AccountTransfer,
        Neft // this is to transfer funds from one bank to another bank.

    }
}
