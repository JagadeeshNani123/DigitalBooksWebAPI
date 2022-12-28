using DigitalBooksWebAPI.Models;
using DigitalBooksWebAPI.Services;

namespace UserService.Services
{
    public interface IUserService
    {
        List<BookMasterViewModel> SearchBook(string title, string categoryName, string authorName, string publisherName, decimal price);
        bool SignIn(string userName, string password);
        string SignUp(User user);
    }
}