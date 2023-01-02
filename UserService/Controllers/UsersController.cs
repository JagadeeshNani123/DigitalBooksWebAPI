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

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly DigitalBooksWebApiContext _context;
        IConfiguration _configuration;

        public UsersController(DigitalBooksWebApiContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            user.Password = PasswordEncryptionAndDecryption.EncodePasswordToBase64(user.Password);
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("SignUp")]
        public async Task<ActionResult<User>> SignUp(User user)
        {

            user.Password = PasswordEncryptionAndDecryption.EncodePasswordToBase64(user.Password);

            try
            {
                user.UserId = Guid.NewGuid();
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {

                throw new Exception("Error occured while adding your account. Please check the details and try again");

            }
            var role = _context.Roles.FirstOrDefault(u => u.RoleId == user.RoleId);
            var roleName = (role != null) ? role.RoleName : string.Empty;

            return Ok("You successfull registered with us.\n Your user name: " + user.UserName + "\n Your role: " + roleName);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("successfully deleted the user from our records");
        }

        [HttpGet]
        [Route("SearchBook")]
        public List<BookMasterViewModel> SearchBook(string title, string categoryName, string authorName, string publisherName, decimal price)
        {
            List<BookMasterViewModel> lsBookMaster = new List<BookMasterViewModel>();
            if (_context.Books == null)
            {
                return lsBookMaster;
            }

            try
            {
            //    var publisherName = _context.Publishers.Any(p => p.PublisherId == publisherId)
            //        ? _context.Publishers.First(p => p.PublisherId == publisherId).PublisherName : string.Empty;

            //    var categoryName = _context.Categories.Any(p => p.CategoryId == categoryId)
            //        ? _context.Categories.First(p => p.CategoryId == categoryId).CategoryName : string.Empty; 

                lsBookMaster = (from b in _context.Books
                                join users in _context.Users on b.UserId equals users.UserId
                                where b.BookName == title && b.Price <= price && b.Active == true
                                select new
                                {
                                    BookId = b.BookId,
                                    BookName = b.BookName,
                                    Author = users.UserName,
                                    Price = b.Price,
                                    PublishedDate = b.PublishedDate,
                                    PublisherId = b.PublisherId,
                                    BookContent = b.BookContent,
                                    Active = b.Active

                                }).ToList()
                                .Select(x => new BookMasterViewModel()
                                {
                                    Title = title,
                                    Author = authorName,
                                    Publisher = publisherName,
                                    Price = price,
                                    PublishedDate = x.PublishedDate,
                                    CategoryName = categoryName,
                                    BookContent = x.BookContent,
                                    Active = x.Active
                                }) .ToList();
            }
            catch (Exception ex)
            {
                return lsBookMaster;
            }

            return lsBookMaster;
        }

        [HttpPost]
        [Route("SignIn")]
        public bool CheckIsValidUser(string userName, string password)
        {
            var encryptedPassword = PasswordEncryptionAndDecryption.EncodePasswordToBase64(password);
            return (_context.Users?.Any(e => e.UserName == userName && e.Password == encryptedPassword)).GetValueOrDefault();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("validate")]
        public object ValidateUser(UserValidationRequestModel request, ITokenService tokenService)
        {
            var userName = request.UserName;
            var password = PasswordEncryptionAndDecryption.EncodePasswordToBase64( request.Password);
            var loggedUserObject = new UserValidationCheck(_context, userName, password);
            var isValidUser = loggedUserObject.IsValidUser();
            var user = loggedUserObject.GetUser();
            if (isValidUser)
            {
                var token = tokenService.buildToken(_configuration["jwt:key"],
                                                    _configuration["jwt:issuer"],
                                                     new[]
                                                    {
                                                 _configuration["jwt:Aud"]
                                                     },
                                                     userName);

                return new
                {
                    Token = token,
                    User = new { UserName = user.UserName, Role = _context.Roles.First(r=> r.RoleId == user.RoleId).RoleName},
                    ExpiryDate= DateTime.Now.Add(new TimeSpan(20, 30, 0)),
                    IsAuthenticated = true
                };
            }
            return new
            {
                Token = "User is not authenticated",
                User ="Not a valid user",
                IsAuthenticated = false
            };
        }

        [HttpGet("confirm-verify")]
        public ContentResult ConfirmVerify()
        {
            var html = System.IO.File.ReadAllText("./WebForms/Dashboard.cshtml");
            return new ContentResult
            {
                Content = html,
                ContentType = "text/html"
            };
        }


        private bool UserExists(Guid id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
