
using Microsoft.EntityFrameworkCore;
using MyDCBank.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
        public DbSet<AccountModel> accountModels { get; set; }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // Define relationships and other model configurations here
        //    modelBuilder.Entity<User>()
        //        .HasOne(u => u.Customer)
        //        .WithOne(c => c.User)
        //        .HasForeignKey<Customer>(c => c.UserID);
        //}

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Define relationships and other model configurations here
            modelBuilder.Entity<User>()
                .HasOne(u => u.Customer)
                .WithOne(c => c.User)
                .HasForeignKey<Customer>(c => c.UserID);

            // Define custom value converter for AccountType enum
            modelBuilder.Entity<Account>()
                .Property(e => e.AccountType)
                .HasConversion(
                    v => v.ToString(),
                    v => (AccountType)Enum.Parse(typeof(AccountType), v));

            // Configure other entities and their relationships
            // Add configurations here as needed
        }



    }
}
