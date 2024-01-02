// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyDCBank.Data;
using MyDCBank.Models;
using System;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

[Route("api/mydcbank/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly BankDBContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(BankDBContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (await _context.Users.AnyAsync(u => u.UserName == model.UserName || u.Email == model.Email))
        {
            return BadRequest(new { Message = "Username or email is already taken" });
        }

        var user = new User
        {
            UserName = model.UserName,
            Email = model.Email,
            Password = model.Password // In a real application, hash the password before saving it
            // Add other user-related properties
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Registration successful" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == model.UserName && u.Password == model.Password);

        if (user == null)
        {
            return BadRequest(new { Message = "Invalid credentials" });
        }

        var token = GenerateJwtToken(user);

        return Ok(new { Token = token });
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
            // Add other claims as needed
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Issuer"],
            claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
