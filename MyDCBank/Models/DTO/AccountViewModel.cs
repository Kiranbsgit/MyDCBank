namespace MyDCBank.Models.DTO
{
    public class AccountViewModel
    {
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public int AccountID { get; set; }
    }
}
