using MyDCBank.Data;
using Microsoft.EntityFrameworkCore;
using MyDCBank.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MyDCBank.Services;



var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
//Jwt configuration starts here
var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = jwtIssuer,
         ValidAudience = jwtIssuer,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
     };
 });
//Jwt configuration ends here

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


builder.Services.AddCors(option =>
{
    option.AddPolicy("My policy", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});
builder.Services.AddDbContext<BankDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BankConnectionString")));   // this is for the dependency injection for the database
builder.Services.AddScoped<IAccountService, AccountService>();// whereever we create a service class for the controller , we have to decalre it in the service class(this class).

 
    // Other service configurations...

    //// Add ASP.NET Core Identity
    ////builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
    ////{
    ////    // Configure password requirements
    ////    options.Password.RequireDigit = false;
    ////    options.Password.RequireLowercase = false;
    ////    options.Password.RequireUppercase = false;
    ////    options.Password.RequireNonAlphanumeric = false;
    ////    options.Password.RequiredLength = 8;
    ////})
    ////.AddEntityFrameworkStores<BankDBContext>() // Specify your DbContext here
    ////.AddDefaultTokenProviders();

    // Other service configurations...


////Add UserManager<User> service
//builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
//{

////Configure Identity options if needed
//options.Password.RequireDigit = true;
//options.Password.RequireLowercase = true;
////Add more options as needed

// //Set role name to 'admin'
//options.User.RequireUniqueEmail = false;
//options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
//options.User.RequireUniqueEmail = false;

//})
//.AddEntityFrameworkStores<BankDBContext>()
//.AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("My policy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
