using DigitalBooksWebAPI.Models;
using DigitalBooksWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Services;

namespace UserService.Implementations
{
    public class ReaderService : IReaderService
    {
        DigitalBooksWebApiContext _context;
        public ReaderService(DigitalBooksWebApiContext context) 
        { 
            _context = context;
        }
        public List<SubscriptionReport> GetAllSubscriptions(Guid userId)
        {
            var subscriptionReport = new List<SubscriptionReport>();
            var user = _context.Users.FirstOrDefault(user => user.UserId == userId);
            var userName = user != null ? user.UserName : string.Empty;
            var subscriptions = _context.Subscriptions.Where(sub => sub.UserId == userId).ToList();
            foreach(var subscription in subscriptions)
            {
                var book = _context.Books.FirstOrDefault(book => book.BookId == subscription.BookId);
                var bookName = book!=null? book.BookName: string.Empty;
                var report = new SubscriptionReport()
                {
                    BookName= bookName,
                    SubscriptionDate= subscription.SubscriptionDate,
                    SubscriptionId= subscription.SubscriptionId,
                    UserName= userName

                };
                subscriptionReport.Add(report);

            }
            return subscriptionReport;
        }

        public string CancelTheSubscriptionWithin24Hrs(Guid subscriptionId)
        {
            var cancelMessage = "Subscription got cancelled successfully";
            var subscription = _context.Subscriptions.Find(subscriptionId);
            if (subscription != null)
            {
                var subscriptionDatetime = Convert.ToDateTime(subscription.SubscriptionDate);
                var dateDifference = DateTime.Now.Subtract(subscriptionDatetime);
                if (dateDifference.TotalHours <= 24)
                {
                    _context.Subscriptions.Remove(subscription);
                    _context.SaveChanges();
                }
            }
            else
            {
                cancelMessage = "You cant cancel subscription.";
            }
            


            return cancelMessage;
        }

        public string ReadTheBook(Guid bookId)
        {
            var selectedBook = _context.Books.FirstOrDefault(b => b.BookId == bookId);

            var bookContent = selectedBook != null ? selectedBook.BookContent : string.Empty;
            return bookContent;
        }

        public string Subscribe(Guid userId, Guid bookId)
        {
            var book = _context.Books.FirstOrDefault(b => b.BookId == bookId);
            var bookName = book != null ? book.BookName : throw new Exception("book not found");
            Subscription subscription = new Subscription
            {
                SubscriptionId = Guid.NewGuid(),
                BookId = bookId,
                UserId = userId,
                SubscriptionDate = DateTime.Now.ToShortDateString()
            };

            try
            {
                _context.Subscriptions.Add(subscription);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw new Exception("error occured while subscribing the book");
            }
            var message = " successfully subscribed.\n Your book name: " + bookName;
            return message;
        }

        public SubscriptionInvoice GenerateSubscriptionInvoice(Guid subscriptionId)
        {

            var bookName = string.Empty;
            var userName = string.Empty;
            var subscription = _context.Subscriptions.FirstOrDefault(b => b.SubscriptionId == subscriptionId);
            var invoice = new SubscriptionInvoice();
            if (subscription != null)
            {
                var book = _context.Books.FirstOrDefault(book => book.BookId == subscription.BookId);
                if (book != null)
                    bookName = book.BookName;
                var user = _context.Users.FirstOrDefault(user => user.UserId == subscription.UserId);
                if (user != null)
                    userName = user.UserName;

                invoice = new SubscriptionInvoice
                {
                    SubscriptionId = subscription.SubscriptionId,
                    BookName = bookName,
                    ReaderName = userName,
                    SubscriptionDate = subscription.SubscriptionDate
                };
            }

            return invoice;
        }

    }
}
