    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
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

        }
    }
