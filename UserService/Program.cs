using DigitalBooksWebAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserService.JwtToken;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DigitalBooksWebApiContext>(options => options.
UseSqlServer(builder.Configuration.GetConnectionString("conn")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["jwt:Issuer"],
            ValidAudience = builder.Configuration["jwt:Aud"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:key"]))
        };
    });


builder.Services.AddSingleton<ITokenService>(new TokenService());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.MapPost("/validate", [AllowAnonymous] (UserValidationRequestModel request) =>
//{
//    var userName = request.UserName;
//    var password = request.Password;
//    var loggedUserObject = new UserValidationCheck(userName, password);
//    var isValidUser = loggedUserObject.IsValidUser();
//    var user = loggedUserObject.GetUser();
//    if (isValidUser)
//    {
//        var tokenService = new TokenService();
//        var token = tokenService.buildToken(builder.Configuration["jwt:key"],
//                                            builder.Configuration["jwt:issuer"],
//                                             new[]
//                                            {
//                                                 builder.Configuration["jwt:Aud"]
//                                             },
//                                             userName);

//        return new
//        {
//            Token = token,
//            User = user,
//            IsAuthenticated = true
//        };
//    }
//    return new
//    {
//        Token = string.Empty,
//        User = user,
//        IsAuthenticated = false
//    };
//}).WithName("validate");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
