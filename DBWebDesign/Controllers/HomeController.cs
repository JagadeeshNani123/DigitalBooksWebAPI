using DBWebDesign.Models;
using DigitalBooksWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace DBWebDesign.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        HttpClient client = new HttpClient();
        string baseUrl = "https://localhost:7009/api/Users";

        public HomeController(ILogger<HomeController> logger)
        {
            client.BaseAddress = new Uri(baseUrl);
            
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var response = await client.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject(responseBody);
            return View(users);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}