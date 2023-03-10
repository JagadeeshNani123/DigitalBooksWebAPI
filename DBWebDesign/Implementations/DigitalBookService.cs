using DigitalBooksWebAPI.Models;
using DigitalBooksWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Text;

namespace DBWebDesign.Implementations
{
    public class DigitalBookService
    {
        List<User> users = new List<User>();
        List<User> authors = new List<User>();
        List<User> readers = new List<User>();
        List<User> sysAdmins = new List<User>();
        List<Publisher> publishers = new List<Publisher>();
        List<Category> categories = new List<Category>();
        List<Role> roles = new List<Role>();
        List<Book> books = new List<Book>();
        
        public DigitalBookService()
        {
            
            ConfigureUserServiceActionMethods();
        }

        public void ConfigureUserServiceActionMethods()
        {
            var users = GetUsers().Result;
            var roles = GetRoles().Result;
            var books = GetBooks().Result;
            var categories = GetCategories().Result;
            var publishers = GetPublishers().Result;
            authors = GetAuthors();
            readers = GetReaders();
            sysAdmins = GetSysAdmins();
        }

        public async Task<List<User>> GetUsers()
        {
            HttpClient userClient = new HttpClient();
            string baseUrl = "https://localhost:7009/api/Users";
            userClient.BaseAddress = new Uri(baseUrl);
            var response = await userClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var usersListObj = JsonConvert.DeserializeObject<List<User>>(responseBody);
            users = usersListObj != null ? usersListObj : new List<User>();
            return users;
        }

        public async void SignUpUser(User user)
        {
            HttpClient userClient = new HttpClient();
            string baseUrl = "https://localhost:7009/api/Users";
            userClient.BaseAddress = new Uri(baseUrl);
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = baseUrl+"/SignUp";

            var response = await userClient.PostAsync(url, data);
            response.EnsureSuccessStatusCode();
        }

        public List<BookMasterViewModel> SearchBooks(string title, string catName, string autName, string  pubName, decimal prc)
        {
            var allBooks = GetDashboardBooks();
            var filteredBooks = allBooks.Where(b => b.Title == title || b.Price <= prc || b.CategoryName == catName || b.Publisher == pubName).ToList();
            return filteredBooks;
        }

        public async Task<List<Publisher>> GetPublishers()
        {
            HttpClient publishersClient = new HttpClient();
            var publishersBaseUrl = "https://localhost:7012/api/Publishers";
            publishersClient.BaseAddress = new Uri(publishersBaseUrl);
            var response = await publishersClient.GetAsync(publishersBaseUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var publishersListObj = JsonConvert.DeserializeObject<List<Publisher>>(responseBody);
            publishers = publishersListObj != null ? publishersListObj : new List<Publisher>();
            return publishers;
        }

        public async Task<List<Category>> GetCategories()
        {
            HttpClient categoriesClient = new HttpClient();
            var categoriesBaseUrl = "https://localhost:7012/api/Categories";
            categoriesClient.BaseAddress = new Uri(categoriesBaseUrl);
            var response = await categoriesClient.GetAsync(categoriesBaseUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var categoriesListObj = JsonConvert.DeserializeObject<List<Category>>(responseBody);
            categories = categoriesListObj != null ? categoriesListObj : new List<Category>();
            return categories;
        }

        public async Task<List<Book>> GetBooks()
        {
            HttpClient booksClient = new HttpClient();
            var booksBaseUrl = "https://localhost:7012/api/Books";
            booksClient.BaseAddress = new Uri(booksBaseUrl);
            var response = await booksClient.GetAsync(booksBaseUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var booksObj = JsonConvert.DeserializeObject<List<Book>>(responseBody);
            books = booksObj != null ? booksObj : new List<Book>();
            return books;
        }

        public async Task<List<Role>> GetRoles()
        {
            HttpClient rolesClient = new HttpClient();
            var rolesBaseUrl = "https://localhost:7009/api/Roles";
            rolesClient.BaseAddress = new Uri(rolesBaseUrl);
            var response = await rolesClient.GetAsync(rolesBaseUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var rolesListObj = JsonConvert.DeserializeObject<List<Role>>(responseBody);
            roles = rolesListObj != null ? rolesListObj : new List<Role>();
            return roles;
        }

        public  List<User> GetAuthors()
        {
            var author = roles.Count() != 0 ? roles.FirstOrDefault(role => role.RoleName == "Author") : null;
            return author != null ? users.Where(user => user.RoleId == author.RoleId).ToList() : new List<User>();
        }

        public List<User> GetReaders()
        {
            var reader = roles.Count() != 0 ? roles.FirstOrDefault(role => role.RoleName == "Reader") : null;
            return  reader != null ? users.Where(user => user.RoleId == reader.RoleId).ToList() : new List<User>();
        }

        public List<User> GetSysAdmins()
        {
            var systemAdmin = roles.Count() != 0 ? roles.FirstOrDefault(role => role.RoleName == "SystemAdmin") : null;
            return systemAdmin != null ? users.Where(user => user.RoleId == systemAdmin.RoleId).ToList() : new List<User>();
        }
        public List<BookMasterViewModel> GetDashboardBooks()
        {
            var dashBoardBooks = new List<BookMasterViewModel>();
            foreach (var book in books)
            {
                var bookName = book.BookName;
                var authorName = GetAuthorName(book.UserId);
                var publisherName = GetPublisherName(book.PublisherId);
                var price = book.Price;
                var pubDate = book.PublishedDate;
                var categoryName = GetCategoryName(book.CategoryId);
                var bookContent = book.BookContent;
                var active = book.Active;
                var dasboardBook = new BookMasterViewModel()
                {
                    Title = bookName,
                    Author = authorName,
                    Publisher = publisherName,
                    CategoryName = categoryName,
                    PublishedDate = pubDate,
                    BookContent = bookContent,
                    Price = price,
                    Active = active
                };
                dashBoardBooks.Add(dasboardBook);
            }
            return dashBoardBooks;
        }
        public string GetAuthorName(Guid autId)
        {
            var author = users.FirstOrDefault(user => user.UserId == autId);
            var authorName = author != null ? author.UserName : string.Empty;
            return authorName;  
        }

        public string GetCategoryName(Guid catId)
        {
            var category = categories.FirstOrDefault(cat => cat.CategoryId == catId);
            var categoryName = category != null ? category.CategoryName : string.Empty;
            return categoryName;
        }

        public string GetPublisherName(Guid pubId)
        {
            var publisher = publishers.FirstOrDefault(pub => pub.PublisherId == pubId);
            var publisherName = publisher != null ? publisher.PublisherName : string.Empty;
            return publisherName;
        }
        public string GetRoleName(Guid roleId)
        {
            var role = roles.FirstOrDefault(user => user.RoleId == roleId);
            var roleName = role != null ? role.RoleName : string.Empty;
            return roleName;
        }

    }
}
