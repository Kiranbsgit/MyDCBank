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
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using MyDCBank.Models.DTO;
using System.Security.Cryptography;

[Route("api/mydcbank/user")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly BankDBContext _context;
    private readonly IConfiguration _configuration;
    //private readonly UserManager<User> _userManager;
    //private readonly RoleManager<IdentityRole<int>> _roleManager;



    public AuthController(BankDBContext context, IConfiguration configuration) //, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager
    {
        _context = context;
        _configuration = configuration;
        //_userManager = userManager;
        //_roleManager = roleManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationModel userObj)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (await _context.Users.AnyAsync(u => u.UserName == userObj.UserName || u.Email == userObj.Email))
        {
            return BadRequest(new { Message = "Username or email is already taken" });
        }

        var user = new User
        {
            UserName = userObj.UserName,
            FirstName = userObj.FirstName,
            LastName = userObj.LastName,
            Email = userObj.Email,
            Address = userObj.Address,
            PhoneNumber = userObj.PhoneNumber,
            ZipCode = userObj.ZipCode,
            Role = UserRoles.UserRole

            // In a real application, hash the password before saving it
            // Add other user-related properties
        };

        string NewHashedPassword =  HashPassword( userObj.Password);

        user.Password = NewHashedPassword;

        if (NewHashedPassword == null)
        {
            return BadRequest(new { Message = "Failed to create user" });
        }
        



        // to insert values into customer table
        var customer = new Customer
        {
            UserName= userObj.UserName,
            FirstName = userObj.FirstName,
            LastName = userObj.LastName,
            DateOfBirth = userObj.DateOfBirth,
            Email = userObj.Email,
            PhoneNumber = userObj.PhoneNumber,
            Address = userObj.Address,
            
            

        };
      
        // security ans and question will be saved here
        var securityInfo = new SecurityInfo
        {
          SecurityQuestion= userObj.SecurityQuestion,
          SecurityAnswer = userObj.SecurityAnswer

        };

        user.Customer = customer;   // understand what these two line of code means in depth!!!!!!!!!
        var userWithCustomer = _context.Users.Include(u => u.Customer).FirstOrDefault();
        _context.Users.Add(user);  
        //---------------------------------------------
        /*In the Register action of your AuthController, 
         * you are using ASP.NET Core Identity's UserManager to create the user,
         * which internally handles adding the user to the database.
         * When you call _userManager.CreateAsync(user, userObj.Password),
         * it creates the user in the ASP.NET Core Identity system and persists it to the database.

        So, explicitly adding _context.Users.Add(user) after creating the user with _userManager.CreateAsync is redundant because CreateAsync method already handles adding the user to the database.
         * --------------------------------------------------------------------------------------------
         */
        _context.Customers.Add(customer);
        _context.securityInfo.Add(securityInfo);
       

        await _context.SaveChangesAsync();
    
        return Ok(new { Message = "Registration successful" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginModel userObj)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        //var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userObj.UserName && u.Password == userObj.Password);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userObj.UserName);
        var customer = await _context.Customers.FirstOrDefaultAsync(v=>v.UserName==userObj.UserName);
       

        if (user == null)
        {
            return BadRequest(new { Message = "User Does not exist!!" });
        }

        if (!VerifyPassword(userObj.Password, user.Password))
        {
            return BadRequest(new { Message = "Invalid username or password" });
        }

         string role = user.Role; // Get the user's role
        int CustomerID = customer.CustomerID; // get customerId to retrieve the customer details in my profile.
       


        //if (this.User.Password!=userObj.Password)
        //{
        //    return Unauthorized(new { Message = "Please enter correct Username or password!!." });
        //}
        //if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
        //{
        //    return BadRequest(new { Message = "Password is Incorrect" });
        //}
        //---------------------admin token generation and login------------------------------//
       

        
        if (role == "admin")
        {
            // You can add custom logic here for handling admin login
            // For example, checking if the user is in the admin role
            // and generating a special token for admin
            var adminToken = GenerateAdminJwtToken(user);
            var adminAccessToken = adminToken;
            var adminRefreshToken = CreateRefreshToken();
            user.RefreshToken = adminRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(5);
          
            var adminTokens = new TokenApiDto()
            {
                AccessToken = adminAccessToken,
                RefreshToken = adminRefreshToken
            };


            return Ok(new
            {
                message = "Logged in as admin successfully",
                token = adminTokens,
                role = role,
                CustomerID = CustomerID
            });
        }
        //-------------------------user token generation -------------------------------//

        var token = GenerateJwtToken(user);
        var newAccessToken = token;
        var newRefreshToken = CreateRefreshToken();
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(5);
        await _context.SaveChangesAsync();
         var tokens= new TokenApiDto()
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };


        return Ok(new 
        {
            message = "Logged in successfully",
            token = tokens,
            role= role,
            CustomerID=CustomerID

            
        }

        );      
    }

    //--------------------------------********TOken generation methods************************_---------------------------------------\\

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
           {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                // Add other claims as needed
            };

        // Retrieve secret key from configuration
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var Sectoken = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Issuer"],
              null);
        // Define token descriptor
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        // Create JWT token
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // Serialize token to string
        return tokenHandler.WriteToken(token);
    }
    private string CreateRefreshToken()
    {
        var tokenBytes = RandomNumberGenerator.GetBytes(64);
        var refreshToken = Convert.ToBase64String(tokenBytes);

        var tokenInUser = _context.Users
            .Any(a => a.RefreshToken == refreshToken);
        if (tokenInUser)
        {
            return CreateRefreshToken();
        }
        return refreshToken;
    }


    private string GenerateAdminJwtToken(User user)
    {
        var claims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
        new Claim(ClaimTypes.Name, user.UserName),
        // Add other claims as needed
        new Claim("IsAdmin", "true") // Example claim indicating admin status
    };

        // Retrieve secret key from configuration
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        // Define token descriptor
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        // Create JWT token
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // Serialize token to string
        return tokenHandler.WriteToken(token);
    }

    //------------------------------------***********************************************__________________________________________________________\\
    private const int SaltSize = 16; // Change as needed
    private const int HashSize = 20; // Change as needed
    private const int Iterations = 10000; // Change as needed

    public static string HashPassword(string password)
    {
        byte[] salt;
        using (var rngCsp = new RNGCryptoServiceProvider())
        {
            rngCsp.GetBytes(salt = new byte[SaltSize]);
        }

        using (var key = new Rfc2898DeriveBytes(password, salt, Iterations))
        {
            byte[] hash = key.GetBytes(HashSize);

            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            string HashedPassword = Convert.ToBase64String(hashBytes);

            return HashedPassword;
        }
    }

    public static bool VerifyPassword(string password, string base64Hash)
    {
        byte[] hashBytes = Convert.FromBase64String(base64Hash);

        if (hashBytes.Length != SaltSize + HashSize)
        {
            return false; // Invalid hash length
        }

        byte[] salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);

        using (var key = new Rfc2898DeriveBytes(password, salt, Iterations))
        {
            byte[] hash = key.GetBytes(HashSize);

            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false; // Hash mismatch
                }
            }
            return true; // Password verified
        }
    }

    public static string AddRole(string UserRole)
    {

        return null ;
    }

}
    
    
