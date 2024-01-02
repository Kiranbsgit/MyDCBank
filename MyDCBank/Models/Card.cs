using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDCBank.Models
{
    public class Card
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CardID { get; set; }

        [DataType(DataType.CreditCard)]
        [Required(ErrorMessage = "Cardholder name is required.")]
        public string CardHolderName { get; set; }


        [Required]
        [MaxLength(16)]
        public string CardNumber { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [MaxLength(3)]
        public string CVV { get; set; }

        [Required]
        [MaxLength(20)]
        public CardType Type { get; set; }

        // Foreign key
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }

        // Navigation property
        public Customer Customer { get; set; }
    }
}
