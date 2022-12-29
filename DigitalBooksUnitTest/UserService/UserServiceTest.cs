using Moq;
using AutoFixture;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Services;
using NUnit.Framework;
using DigitalBooksWebAPI.Services;
using DigitalBooksWebAPI.Models;

namespace DigitalBooksUnitTest.UserService
{
    public class UserServiceTest
    {
        private readonly Mock<IUserService> _userService;
        public UserServiceTest()
        {
            _userService = new Mock<IUserService>();
        }
       
        [Theory]
        public void Test_SearchBook()
        {
            //Arrange
            _userService.Setup(bs => bs.SearchBook(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>())).Returns(new List<BookMasterViewModel>());

            //Act
            var result = _userService.Object.SearchBook("title","categoryName", "authorName", "publisherName",123.45m);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<BookMasterViewModel>>();
        }

        [Theory]
        public void Test_SignIn()
        {
            //Arrange
            _userService.Setup(bs => bs.SignIn(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            //Act
            var result = _userService.Object.SignIn("userName", "password");

            //Assert
            result.Should().BeTrue();
        }

        [Theory]
        public void Test_SignUp()
        {
            //Arrange
            _userService.Setup(bs => bs.SignUp(It.IsAny<User>())).Returns("success");

            //Act
            var result = _userService.Object.SignUp(new User());

            //Assert
            result.Should().Be("success");
            result.Should().BeAssignableTo<string>();
        }
    }
}
