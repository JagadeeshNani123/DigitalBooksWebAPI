using DigitalBooksWebAPI.Models;
using DigitalBooksWebAPI.Services;

namespace UserService.Services
{
    public interface IReaderService
    {
        string CancelTheSubscriptionWithin24Hrs(Guid subscriptionId);
        SubscriptionInvoice GenerateSubscriptionInvoice(Guid subscriptionId);
        List<SubscriptionReport> GetAllSubscriptions(Guid userId);
        string ReadTheBook(Guid bookId);
        string Subscribe(Guid userId, Guid bookId);
    }
}