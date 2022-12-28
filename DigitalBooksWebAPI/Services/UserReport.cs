using DigitalBooksWebAPI.Models;

namespace DigitalBooksWebAPI.Services
{
    public class UserReport
    {
        public string UserName { get; set; } 

        public string RoleName { get; set; }

        public List<Book> books { get; set; }
    }
}
