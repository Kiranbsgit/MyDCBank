using MyDCBank.Data;
using MyDCBank.Models;
using MyDCBank.Services;

namespace MyDCBank.Services
{
    public class UserServices : IUserServices
    {
        private readonly BankDBContext _context;
        public UserServices(BankDBContext context)
        {
                this._context = context;
        }

       
    }
}
