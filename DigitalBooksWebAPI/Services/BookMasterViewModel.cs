namespace DigitalBooksWebAPI.Services
{
    public class BookMasterViewModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public decimal Price { get; set; }
        public string PublishedDate { get; set; }
        public string CategoryName { get; set; }
        public string BookContent { get; set; }
        public bool Active { get; set; }
    }
}
