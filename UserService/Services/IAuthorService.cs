using DigitalBooksWebAPI.Models;

namespace UserService.Services
{
    public interface IAuthorService
    {
        string CreateBook(Book book);
        string EnabledOrDisableTheBook(Guid bookId, bool isDisabled);
        List<Book> GetAuthorBooks(Guid autorId);
    }
}