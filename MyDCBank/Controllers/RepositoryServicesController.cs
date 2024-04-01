    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDCBank.Data;
using MyDCBank.Models;
    using MyDCBank.Models.DTO;
    using MyDCBank.Services;

    namespace MyDCBank.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class RepositoryServicesController : ControllerBase
        {
            private readonly BankDBContext _context;

            public RepositoryServicesController(BankDBContext context)
            {
                    this._context = context;
            }

            [HttpGet("Customers")]
            public async Task<ActionResult<CustomerViewModel>> GetCustomer(int CustomerID)
            {
                var customer = await _context.Customers.FindAsync(CustomerID);

                if (customer == null)
                {
                    return NotFound();
                }
                var customerViewModel = new CustomerViewModel
                {
                    UserName = customer.UserName,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    DateOfBirth = customer.DateOfBirth,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber,
                    Address = customer.Address
                };

                return customerViewModel;
            }
        [HttpGet("Account")]
        public async Task<ActionResult<AccountViewModel>>GetAccounts(int customerID)
        {
            var customer = await _context.Customers.FindAsync(customerID);

            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            var accounts = _context.accounts.Where(a => a.CustomerID == customerID).ToList();

            if (accounts == null || accounts.Count == 0)
            {
                return NotFound("No accounts found for this customer.");
            }

            var accountViewModels = accounts.Select(account => new AccountViewModel
            {
                AccountName = account.AccountName,
                AccountNumber = account.AccountNumber,
                AccountID = account.AccountID,
                AccountType = account.AccountType.ToString(),
                Balance = account.Balance,
            });

            return Ok(accountViewModels);
        }


        [HttpGet("FindUser")]
        public async Task<ActionResult<FindUserViewModel>>FindUser(string userName)
        {

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserName == userName);

            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            var foundUser = new FindUserViewModel
            {
                CustomerID = customer.CustomerID,
                // Include any other properties you need to return in the view model
            };

            return Ok(foundUser);
        }


    }
    }
