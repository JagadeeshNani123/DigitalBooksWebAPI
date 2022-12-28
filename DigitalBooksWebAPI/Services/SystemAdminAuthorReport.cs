using DigitalBooksWebAPI.Models;

namespace DigitalBooksWebAPI.Services
{
    public class SystemAdminAuthorReport
    {
        public string PublisherName { get; set; }

        public string publishedDate { get; set; }

        public string BookName { get; set; }

        public List<User> Readers { get; set; }

        public string AuthorName { get; set; }
    }
}
