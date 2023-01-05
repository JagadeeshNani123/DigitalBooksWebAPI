using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DigitalBooksWebAPI.Models;
using DigitalBooksWebAPI.Services;
using System.Security.Policy;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using UserService.JwtToken;
using UserService.Implementations;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly DigitalBooksWebApiContext _context;
        IConfiguration _configuration;
        UsersService usersService;

        public UsersController(DigitalBooksWebApiContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            usersService = new UsersService(_context, _configuration);
        }
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_context.Users.Count() == 0)
            {
                return Ok("No records found");
            }
            else
                return await _context.Users.ToListAsync();
        }


        [HttpPost]
        [Route("SignUp")]
        public async Task<ActionResult<User>> SignUp(User user)
        {
            var message = usersService.SignUp(user);
            return Ok(message);
        }

        // DELETE: api/Users/5


        [HttpGet]
        [Route("SearchBook")]
        public List<BookMasterViewModel> SearchBook(string title, string categoryName, string authorName, string publisherName, decimal price)
        {
            var bookList = usersService.SearchBook(title, categoryName, authorName, publisherName, price);
            return bookList;
        }

        [HttpPost]
        [Route("SignIn")]
        public bool CheckIsValidUser(string userName, string password)
        {
            var isValidUser = usersService.CheckIsValidUser(userName, password);
            return isValidUser;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("validate")]
        public object ValidateUser(UserValidationRequestModel request)
        {
            var tokenValidationObj = usersService.ValidateUser(request);
            return tokenValidationObj;
        }


    }
}

