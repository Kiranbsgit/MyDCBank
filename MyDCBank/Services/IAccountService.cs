using MyDCBank.Models;


namespace MyDCBank.Services
{
    public interface IAccountService
    {
        Account CreateAccountForUser( AccountModel newObj);
    }
}
