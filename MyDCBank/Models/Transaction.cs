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
        public string TransactionType { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount   { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }


        [ForeignKey("AccountID")]
        public int AccountID { get; set; }
        // Navigation property
        public Account Account { get; set; }
        
        
    }
}
