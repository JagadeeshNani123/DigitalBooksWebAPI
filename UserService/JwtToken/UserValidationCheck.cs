using DigitalBooksWebAPI.Models;

namespace UserService.JwtToken
{
    public class UserValidationCheck
    {
        private readonly DigitalBooksWebApiContext _context;
        public string UserName { get; set; }
        public string Password { get; set; }


        
        public UserValidationCheck(DigitalBooksWebApiContext context, string userName, string password)
        {
            _context = context;
            UserName = userName;
            Password = password;
        }


        public User GetUser()
        {
            return _context.Users.FirstOrDefault(user => user.UserName == UserName && user.Password == Password);
        }
        public bool IsValidUser()
        {
            return _context.Users.Any(user => user.UserName == UserName && user.Password == Password);
        }
    }
}
