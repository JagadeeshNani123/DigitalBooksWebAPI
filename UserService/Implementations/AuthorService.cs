using DigitalBooksWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using UserService.Services;

namespace UserService.Implementations
{
    public class AuthorService : IAuthorService
    {
        DigitalBooksWebApiContext _context;
        public AuthorService(DigitalBooksWebApiContext context)
        {
            _context = context;
        }

        public List<Book> GetAuthorBooks(Guid autorId)
        {
            var authorBooks = _context.Books.Where(books => books.UserId == autorId).ToList();
            return authorBooks;
        }

        public string CreateBook(Book book)
        {
            try
            {
                book.BookId = Guid.NewGuid();
                _context.Books.Add(book);
                _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new Exception("error occured while creating the book");
            }

            return "You successfull created book.\n Your book name: " + book.BookName;
        }

        public string EnabledOrDisableTheBook(Guid bookId, bool isDisabled)
        {
            var book = _context.Books.FirstOrDefault(book => book.BookId == bookId);
            if (book != null)
            {
                book.Active = isDisabled;
            }
            var messageToReader = isDisabled ? "This book temporarly disabled by author" : string.Empty;
            return messageToReader;
        }
    }
}
