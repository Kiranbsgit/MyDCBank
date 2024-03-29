using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDCBank.Data;
using MyDCBank.Models;
using MyDCBank.Services;
using System;

    namespace MyDCBank.Controllers
    {
        [Route("api/MyDCBank")]
        [ApiController]
        public class AccountServiceController : ControllerBase
        {
        private readonly IAccountService _accountService;
        private readonly BankDBContext _context;
        

        public AccountServiceController(IAccountService accountService, BankDBContext context)
        {
            _accountService = accountService;
            _context = context;

        }

        [HttpPost("createaccount")]
        public async Task<IActionResult> CreateAccountForUser( [FromBody] AccountModel newObj)
            {
            try
            {
                
                //if (await _context.Customers.AnyAsync(u => u.CustomerID == newObj.CustomerID ))
                //{
                //    return BadRequest(new { Message = "Please enter correct CustomerID!!!" });
                //};
                // Generate account for the specified user ID and account type
                var accountDetails = _accountService.CreateAccountForUser(newObj);

                if (accountDetails == null)
                {
                    return NotFound("User not found.");
                }

                return Ok(new { Message = " Account registration successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}
