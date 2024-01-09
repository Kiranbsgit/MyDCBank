
using Microsoft.EntityFrameworkCore;
using MyDCBank.Models;

namespace MyDCBank.Data

{
    public class BankDBContext:DbContext
    {
        public BankDBContext(DbContextOptions options): base(options)
        {

        }
        public DbSet<Account> accounts { get; set; }
        public DbSet<Card> cards { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserLoginModel> userLoginModels { get; set; }
        public DbSet<UserRegistrationModel> userRegistrationModels { get; set; }
        public DbSet<SecurityInfo> securityInfo { get; set; }


    }
}
