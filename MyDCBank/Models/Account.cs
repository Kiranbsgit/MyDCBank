﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDCBank.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountID { get; set; }

        [Required]
        [MaxLength(20)]
        public string AccountName { get; set; }


        // this is of type enum --- savings, current, salary and government account types.
        [Required]
        [MaxLength(20)]
        [Column(TypeName ="nvarchar(50)")]
        public AccountType AccountType { get; set; }

        // account number should be auto generated by the system for every customer who will create an account.
        [Required(ErrorMessage = "Account number is required.")]      
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }


        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        [Required]
        public DateTime OpenDate   { get; set; }

        // Additional property for error message

        


        // Foreign key
        public int CustomerID { get; set; }

        // Navigation property
        [ForeignKey("CustomerID")]
        public Customer Customer { get; set; }

        //// Additional properties to store first name and last name from Customer
        //public string CustomerFirstName { get; set; }

        //public string CustomerLastName { get; set; }

        // Navigation property  
        public List<Card> Cards { get; set; } = new List<Card>();   
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        //-----------------------------------------------------------------------------

        // Constructor to copy properties from associated Customer
        //public Account(Customer customer)
        //{
        //    if (customer != null)
        //    {
        //        CustomerFirstName = customer.FirstName;
        //        CustomerLastName = customer.LastName;
        //        PhoneNumber = customer.PhoneNumber;
        //        Email = customer.Email;
        //    }
        //}

        //let's generate account number by creating a constructor
        // Let's create a random object
        //Random rand = new Random();

        //public Account()
        //{
        //    AccountNumber=Convert.ToString((long)rand.NextDouble()* 9_000_000_000L + 1_000_000_000L);
        //    AccountName = $"{CustomerFirstName} {CustomerLastName}";
        //}

        //// Constructor to copy first name and last name from associated Customer
       
    }

    public enum AccountType
    {
        savings,
        current,
        salary,
        Government
    }


    
}
