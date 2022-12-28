using DigitalBooksWebAPI.Models;
using DigitalBooksWebAPI.Services;

namespace UserService.Services
{
    public interface ISystemAdminService
    {
        SystemAdminAuthorReport GenerateSystemAdminAuthorReport(Guid bookId);
        List<SystemAdminReaderReport> GenerateSystemAdminReaderReport();
        List<UserReport> GenerateUserReport();
        List<User> GetSubscribedReaders(Guid bookId);
        List<Subscription> GetSubscriptionsByBookId(Guid bookId);
    }
}