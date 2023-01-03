using DBWebDesign.Implementations;
using DigitalBooksWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Ocelot.Values;

namespace DBWebDesign.Controllers
{
    public class UserServiceController : Controller
    {
        HttpClient client = new HttpClient();
        List<User> users = new List<User>();
        List<User> authors = new List<User>();
        List<User> readers = new List<User>();
        List<User> sysAdmins = new List<User>();
        List<Role> roles = new List<Role>();
        List<Book> books= new List<Book>();
        string baseUrl = "https://localhost:7009/api/Users";
        DigitalBookService dbService = new DigitalBookService();
        dynamic obj = "";
        // GET: UserServiceController
        public UserServiceController()
        {
            LoadSearchBookParameters();
        }
        public void LoadSearchBookParameters()
        {
            GetUsers();
            GetRoles();
            var books = GetBooks();
            var publishers =GetPublishers();
            var categories = GetCategories();
            var authors = GetAuthors();
            var obj = new  
            {
                 Books = books,
                 Publishers= publishers,
                 Categories = categories,
                 Authors = authors
            };
        }

        public List<Book> GetBooks()
        {
            var books =  dbService.GetBooks().Result;
            return books.ToList();
        }
        public  List<Category> GetCategories()
        {
            var categories =  dbService.GetCategories().Result;
            return categories.ToList();
        }

        public List<Publisher> GetPublishers()
        {
            var publishers =  dbService.GetPublishers().Result;
            return  publishers.ToList();
        }

        public async Task<IActionResult> Dashboard(string sortOrder)
        {

            var dashBoardBooks = await dbService.GetDashboardBooks();
            ViewData["PriceSortParm"] = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewData["PublishedDateSortParm"] = sortOrder == "PublishedDate" ? "date_desc" : "PublishedDate";
            ViewData["TitleSortParm"] = sortOrder == "Title" ? "title_desc" :"Title";
            ViewData["AuthorSortParm"] = sortOrder == "Author" ? "author_desc" : "Author";
            ViewData["PublisherSortParm"] = sortOrder == "Publisher" ? "publisher_desc" : "Publisher";
            ViewData["CategorySortParm"] = sortOrder == "CategoryName" ? "categoryName_desc" : "CategoryName";
            ViewData["BookContentSortParm"] = sortOrder == "BookContent" ? "bookContent_desc" : "BookContent";
            ViewData["ActiveOrDeactiveSortParm"] = sortOrder == "Active" ? "active_desc" : "Active";

            switch (sortOrder)
            {
                case "price_desc":
                    dashBoardBooks = dashBoardBooks.OrderByDescending(s => s.Price).ToList();
                    break;
                case "PublishedDate":
                    dashBoardBooks = dashBoardBooks.OrderBy(s => s.PublishedDate).ToList();
                    break;
                case "date_desc":
                    dashBoardBooks = dashBoardBooks.OrderByDescending(s => s.PublishedDate).ToList();
                    break;

                case "title_desc":
                    dashBoardBooks = dashBoardBooks.OrderByDescending(s => s.Title).ToList();
                    break;
                case "Title":
                    dashBoardBooks = dashBoardBooks.OrderBy(s => s.Title).ToList();
                    break;
                case "author_desc":
                    dashBoardBooks = dashBoardBooks.OrderByDescending(s => s.Author).ToList();
                    break;
                case "Author":
                    dashBoardBooks = dashBoardBooks.OrderBy(s => s.Author).ToList();
                    break;
                case "publisher_desc":
                    dashBoardBooks = dashBoardBooks.OrderByDescending(s => s.Publisher).ToList();
                    break;
                case "Publisher":
                    dashBoardBooks = dashBoardBooks.OrderBy(s => s.Publisher).ToList();
                    break;
                case "categoryName_desc":
                    dashBoardBooks = dashBoardBooks.OrderByDescending(s => s.CategoryName).ToList();
                    break;
                case "CategoryName":
                    dashBoardBooks = dashBoardBooks.OrderBy(s => s.CategoryName).ToList();
                    break;
                case "bookContent_desc":
                    dashBoardBooks = dashBoardBooks.OrderByDescending(s => s.BookContent).ToList();
                    break;
                case "BookContent":
                    dashBoardBooks = dashBoardBooks.OrderBy(s => s.BookContent).ToList();
                    break;
                case "active_desc":
                    dashBoardBooks = dashBoardBooks.OrderByDescending(s => s.Active).ToList();
                    break;
                case "Active":
                    dashBoardBooks = dashBoardBooks.OrderBy(s => s.Active).ToList();
                    break;
                default:
                    dashBoardBooks = dashBoardBooks.OrderBy(s => s.Price).ToList();
                    break;
            }


           
            ViewBag.DashboardBooks = dashBoardBooks;
            return View(dashBoardBooks);
        }

        // GET: UserServiceController/Create
        public ActionResult Register()
        {
            return View();
        }

        // POST: UserServiceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserServiceController/Edit/5
        public ActionResult LogIn()
        {
            return View();
        }

        // POST: UserServiceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserServiceController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserServiceController/Delete/5
      

        public async Task<IActionResult> ConfigureUserServiceActionMethods()
        {
            var response = await client.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            string responseBody =  response.Content.ReadAsStringAsync().Result;
            var usersListObj = JsonConvert.DeserializeObject(responseBody);
            var users = usersListObj != null ? (List<User>)usersListObj : new List<User>();
            return View(users);
        }

        public async void GetUsers()
        {
            var response = await client.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            string responseBody =  response.Content.ReadAsStringAsync().Result;
            var usersListObj = JsonConvert.DeserializeObject<List<User>>(responseBody);
            users = usersListObj != null ? usersListObj : new List<User>();
            ViewBag.Users = users;
        }

        public async void GetRoles()
        {
            var rolesBaseUrl = "https://localhost:7009/api/Roles";
            client.BaseAddress= new Uri(rolesBaseUrl);
            var res =  client.GetAsync(baseUrl).Result;
            var response = await client.GetAsync(rolesBaseUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var rolesListObj = JsonConvert.DeserializeObject<List<Role>>(responseBody);
            roles = rolesListObj != null ? rolesListObj : new List<Role>();
            ViewBag.Roles = roles;
        }

        public List<User> GetAuthors()
        {
            var author = roles.Count() != 0 ? roles.FirstOrDefault(role => role.RoleName == "Author") : null;
            authors = author!=null ? users.Where(user=> user.RoleId == author.RoleId).ToList() : new List<User>();
            ViewBag.Authors = authors; 
            return authors;
        }

        public async void GetReaders()
        {
            var reader = roles.Count() != 0 ? roles.FirstOrDefault(role => role.RoleName == "Reader") : null;
            readers = reader != null ? users.Where(user => user.RoleId == reader.RoleId).ToList() : new List<User>();
        }

        public async void GetSysAdmins()
        {
            var systemAdmin = roles.Count() != 0 ? roles.FirstOrDefault(role => role.RoleName == "SystemAdmin") : null;
            sysAdmins = systemAdmin != null ? users.Where(user => user.RoleId == systemAdmin.RoleId).ToList() : new List<User>();
        }
    }
}
