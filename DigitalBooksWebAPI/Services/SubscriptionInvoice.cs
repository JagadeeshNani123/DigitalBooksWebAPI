namespace DigitalBooksWebAPI.Services
{
    public class SubscriptionInvoice
    {
        public Guid SubscriptionId { get; set; }

        public string BookName { get; set; }

        public string ReaderName { get; set; }

        public string SubscriptionDate { get; set; }
    }
}
