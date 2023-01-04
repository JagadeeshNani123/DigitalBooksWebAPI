using DigitalBooksWebAPI.Models;
using DigitalBooksWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using UserService.JwtToken;
using UserService.Services;

namespace UserService.Implementations
{
    public class UsersService : IUserService
    {
        DigitalBooksWebApiContext _context;
        IConfiguration _configuration;
        public UsersService(DigitalBooksWebApiContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string SignUp(User user)
        {

            user.Password = PasswordEncryptionAndDecryption.EncodePasswordToBase64(user.Password);

            try
            {
                user.UserId = Guid.NewGuid();
                _context.Users.Add(user);
                _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {

                throw new Exception("Error occured while adding your account. Please check the details and try again");

            }
            var role = _context.Roles.FirstOrDefault(u => u.RoleId == user.RoleId);
            var roleName = (role != null) ? role.RoleName : string.Empty;

            return "You successfull registered with us.\n Your user name: " + user.UserName + "\n Your role: " + roleName;
        }

        public List<BookMasterViewModel> SearchBook(string title, string categoryName, string authorName, string publisherName, decimal price)
        {


            List<BookMasterViewModel> lsBookMaster = new List<BookMasterViewModel>();
            if (_context.Books == null)
            {
                return lsBookMaster;
            }

            try
            {
                lsBookMaster = (from b in _context.Books
                                join users in _context.Users on b.UserId equals users.UserId
                                where (b.BookName == title || users.UserName == authorName
                                 || b.Price == price)
                                && b.Active == true
                                select new
                                {
                                    BookId = b.BookId,
                                    BookName = b.BookName,
                                    Author = users.UserName,
                                    Price = b.Price,
                                    PublishedDate = b.PublishedDate,
                                    PublisherId = b.PublisherId,
                                    BookContent = b.BookContent,
                                    Active = b.Active

                                }).ToList()
                                .Select(x => new BookMasterViewModel()
                                {
                                    Title = title,
                                    Author = authorName,
                                    Publisher = publisherName,
                                    Price = price,
                                    PublishedDate = x.PublishedDate,
                                    CategoryName = categoryName,
                                    BookContent = x.BookContent,
                                    Active = x.Active
                                }).ToList();
            }
            catch (Exception ex)
            {
                return lsBookMaster;
            }

            return lsBookMaster;
        }

        public bool SignIn(string userName, string password)
        {
            var encryptedPassword = PasswordEncryptionAndDecryption.EncodePasswordToBase64(password);
            return (_context.Users?.Any(e => e.UserName == userName && e.Password == encryptedPassword)).GetValueOrDefault();
        }

        public object ValidateUser(UserValidationRequestModel request)
        {
            var userName = request.UserName;
            var password = PasswordEncryptionAndDecryption.EncodePasswordToBase64(request.Password);
            var loggedUserObject = new UserValidationCheck(_context, userName, password);
            var isValidUser = loggedUserObject.IsValidUser();
            var user = loggedUserObject.GetUser();
            if (isValidUser)
            {
                var tokenService = new TokenService();
                var token = tokenService.buildToken(_configuration["jwt:key"],
                                                    _configuration["jwt:issuer"],
                                                     new[]
                                                    {
                                                 _configuration["jwt:Aud"]
                                                     },
                                                     userName);

                return new
                {
                    Token = token,
                    User = new { UserName = user.UserName, Role = _context.Roles.First(r => r.RoleId == user.RoleId).RoleName },
                    ExpiryDate = DateTime.Now.Add(new TimeSpan(20, 30, 0)),
                    IsAuthenticated = true
                };
            }
            return new
            {
                Token = "User is not authenticated",
                User = "Not a valid user",
                IsAuthenticated = false
            };
        }
        public bool CheckIsValidUser(string userName, string password)
        {
            var encryptedPassword = PasswordEncryptionAndDecryption.EncodePasswordToBase64(password);
            return (_context.Users?.Any(e => e.UserName == userName && e.Password == encryptedPassword)).GetValueOrDefault();
        }
    }
}
