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

        [StringLength(4, MinimumLength = 4)]
        public string Pin { get; set; }

        [Required]
        [MaxLength(20)]
        public CardType Type { get; set; }

        //foreign key
        [ForeignKey("AccountID")]
        public Account Account { get; set; }

        // Navigation property  // not confirmed navigation property
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }

        // in order to perform transaction from the cardholder , this relationship has been established.
        
        public Card(Customer customer)
        {
            if (customer != null) 
            { 
             CustomerFirstName= customer.FirstName;
             CustomerLastName= customer.LastName;

            }
        }
        Random random = new Random();
        public Card()
        {
          CardNumber = Convert.ToString((long)random.NextDouble() *9_000_000_000_000_000L + 1_000_000_000L);
          CVV = Convert.ToString((short)random.NextDouble() *100 + 100);
            CardHolderName = $"{CustomerFirstName}{CustomerLastName}";
        }
    }
    
    public enum CardType
    {
        Credit,
        Debit,
        Luxury,
        Contactless
       
    }



}
