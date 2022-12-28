using DigitalBooksWebAPI.Models;
using DigitalBooksWebAPI.Services;
using System.Net;
using UserService.Services;

namespace UserService.Implementations
{
    public class SystemAdminService : ISystemAdminService
    {
        DigitalBooksWebApiContext _context;
        public SystemAdminService(DigitalBooksWebApiContext context)
        {
            _context = context;
        }

        public SystemAdminAuthorReport GenerateSystemAdminAuthorReport(Guid bookId)
        {
            SystemAdminAuthorReport report = new SystemAdminAuthorReport();

            var readers = GetSubscribedReaders(bookId);

            var book = _context.Books.FirstOrDefault(book => book.BookId == bookId);
            if (book != null)
            {
                var author = _context.Users.FirstOrDefault(user => user.UserId == book.UserId);
                var authorName = author != null ? author.UserName : string.Empty;
                var publisher = _context.Publishers.FirstOrDefault(pub => pub.PublisherId == book.PublisherId);
                var publisherName = publisher != null ? publisher.PublisherName : string.Empty;
                report = new SystemAdminAuthorReport
                {
                    BookName = book.BookName,
                    AuthorName = authorName,
                    publishedDate = book.PublishedDate,
                    PublisherName = publisherName,
                    Readers = readers
                };

            }
            return report;
        }

        public List<SystemAdminReaderReport> GenerateSystemAdminReaderReport()
        {
            var readers = new List<User>();
            var subscriptions = _context.Subscriptions.ToList();
            var systemAdminReaderReports = new List<SystemAdminReaderReport>();
            var books = _context.Books.ToList();
            foreach (var book in books)
            {
                readers.AddRange(GetSubscribedReaders(book.BookId));
            }

            foreach (var reader in readers)
            {
                var subscription = subscriptions.FirstOrDefault(s => s.UserId == reader.UserId);
                if (subscription != null)
                {
                    var book = _context.Books.FirstOrDefault(b => b.BookId == subscription.BookId);
                    if (book != null)
                    {
                        var bookName = book.BookName;
                        var systemAdminReaderReport = new SystemAdminReaderReport()
                        {
                            BookName = bookName,
                            ReaderName = reader.UserName,
                            SubscriptionDate = subscription.SubscriptionDate
                        };
                        systemAdminReaderReports.Add(systemAdminReaderReport);
                    }
                }
            }
            return systemAdminReaderReports;
        }

        public List<User> GetSubscribedReaders(Guid bookId)
        {
            var subscriptions = GetSubscriptionsByBookId(bookId);
            var readers = new List<User>();
            foreach (var subscription in subscriptions)
            {
                var reader = _context.Users.FirstOrDefault(user => user.UserId == subscription.UserId);
                if (reader != null)
                {
                    if (!readers.Any(r => reader.UserId == reader.UserId))
                        readers.Add(reader);
                }
            }
            return readers;
        }

        public List<Subscription> GetSubscriptionsByBookId(Guid bookId)
        {
            var subscriptions = _context.Subscriptions.Where(sub => sub.BookId == bookId).ToList();

            return subscriptions;
        }

        public List<UserReport> GenerateUserReport()
        {
            var userReport = new List<UserReport>();
            var books = new List<Book>();
            var users = _context.Users.ToList();
            foreach (var user in users)
            {
                var roleName = string.Empty;
                var role = _context.Roles.FirstOrDefault(r => r.RoleId == user.RoleId);
                if (role != null)
                {
                    roleName = role.RoleName;
                    if (roleName == "Author")
                    {
                        books = books.Where(b => b.UserId == user.UserId).ToList();
                    }
                    else if (roleName == "Reader")
                    {
                        var subscriptions = _context.Subscriptions.Where(sub => sub.UserId == user.UserId).ToList();
                        foreach (var subscription in subscriptions)
                        {
                            books.AddRange(_context.Books.Where(book => book.BookId == subscription.BookId).ToList());
                        }
                    }
                }
                var report = new UserReport()
                {
                    books = books,
                    RoleName = roleName,
                    UserName = user.UserName
                };
                userReport.Add(report);
                books = new List<Book>();

            }
            return userReport;

        }
    }
}
