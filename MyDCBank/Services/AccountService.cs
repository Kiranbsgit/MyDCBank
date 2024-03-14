using Microsoft.EntityFrameworkCore;
using MyDCBank.Data;
using MyDCBank.Models;

namespace MyDCBank.Services
{
    public class AccountService : IAccountService
    {
        private readonly BankDBContext _context;

        private static readonly Random rand = new Random();
        public AccountService(BankDBContext context)
        {
            _context = context;
        }
        
        public Account CreateAccountForUser( AccountModel newObj )
        {
            // Retrieve customer details based on the provided customer ID
           var Customer = _context.Customers.FirstOrDefault(c => c.CustomerID == newObj.CustomerID);


           
            //string message = "CustomerId does not exist!!. Please enter correct CustomerID";

            if (Customer == null)
            {
                // User not found, return error message
                return null;
                
            }

            // Generate account details
            var account = new Account
            {
                AccountName = $"{Customer.FirstName} {Customer.LastName}",
                AccountType = newObj.AccountType,
                AccountNumber = GenerateAccountNumber(),
                PhoneNumber = Customer.PhoneNumber,
                Email = Customer.Email,
                Balance = 0,
                OpenDate = DateTime.UtcNow,
                CustomerID = Customer.CustomerID,
              
               
            };

            // Save account details to the database
            _context.accounts.Add(account);
            _context.SaveChanges();

            return account;
        }

     
        private string GenerateAccountNumber()
        {
            // Implement your logic to generate a unique account number
            // Example implementation:

            /*$"{Convert.ToString((long)rand.NextDouble() * 9_000_000_000L + 1_000_000_000L)}";*/
            int firstHalf = rand.Next(10000, 99999); // Generate a random 5-digit number
            int secondHalf = rand.Next(10000, 99999); // Generate another random 5-digit number
            string AccountNumber = $"{firstHalf}{secondHalf}"; // Concatenate the two halves
            return AccountNumber;



        }
    }
}
