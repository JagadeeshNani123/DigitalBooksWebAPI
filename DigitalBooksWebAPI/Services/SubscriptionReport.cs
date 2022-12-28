namespace DigitalBooksWebAPI.Services
{
    public class SubscriptionReport
    {
        public Guid SubscriptionId { get; set; }

        public string BookName { get; set; }

        public string UserName { get; set; }

        public string SubscriptionDate { get; set; } = null!;
    }
}
