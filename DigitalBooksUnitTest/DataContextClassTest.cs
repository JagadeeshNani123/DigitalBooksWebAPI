using DigitalBooksWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Services;

namespace DigitalBooksUnitTest
{
    public class DataContextClassTest
    {
        private readonly Mock<DigitalBooksWebApiContext> _context;
        List<User> users = new List<User>();
        public DataContextClassTest()
        {
            _context = new Mock<DigitalBooksWebApiContext>();
            AddUsers();
        }

        public void AddUsers()
        {
            var contextUserMock = new Mock<DbSet<User>>();
            _context.Setup(s=> s.Users).Returns(contextUserMock.Object);
            
            var reader = new User()
            {
               UserId = Guid.NewGuid(),
               UserName = "Test Reader",
               Password ="Test Reader",
               RoleId = Guid.NewGuid() 
            };
            users.Add(reader);
            var author = new User()
            {
                UserId = Guid.NewGuid(),
                UserName = "Test Author",
                Password = "Test Author",
                RoleId = Guid.NewGuid()
            };
            users.Add(author);
            var sysAdmin = new User()
            {
                UserId = Guid.NewGuid(),
                UserName = "Test SysAdmin",
                Password = "Test SysAdmin",
                RoleId = Guid.NewGuid()
            };
            users.Add(sysAdmin);

            _context.Object.Users.AddRange(users);
        }
        public List<User> GetUsers()
        {
            return users;
        }
    }
}
