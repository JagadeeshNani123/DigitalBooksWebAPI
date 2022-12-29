using DigitalBooksWebAPI.Models;
using DigitalBooksWebAPI.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Services;

namespace DigitalBooksUnitTest.UserService
{
    public class AuthorServiceTest
    {
        private readonly Mock<IAuthorService> _authorService;
        public AuthorServiceTest()
        {
            _authorService = new Mock<IAuthorService>();
        }

        [Theory]
        public void Test_CreateBook()
        {
            //Arrange
            var bookMock = new Mock<Book>();
            _authorService.Setup(aut => aut.CreateBook(It.IsAny<Book>())).Returns("Success");
            
            //Act
            var result = _authorService.Object.CreateBook(bookMock.Object);

            //Assert
            result.Should().Be("Success");
            result.Should().BeAssignableTo<string>();

        }

        [Theory]
        public void Test_GetAuthorBooks()
        {
            //Arrange
            var users = _authorService.Setup(aut => aut.GetAuthorBooks(It.IsAny<Guid>())).Returns(new List<Book>());

            //Act
            var result = _authorService.Object.GetAuthorBooks(Guid.NewGuid());

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<Book>>();

        }

        [Theory]
        public void Test_EnabledOrDisableTheBook_With_True()
        {
            //Arrange
            var users = _authorService.Setup(aut => aut.EnabledOrDisableTheBook(It.IsAny<Guid>(), It.IsAny<bool>())).Returns("True - Book will be disabled");

            //Act
            var result = _authorService.Object.EnabledOrDisableTheBook(Guid.NewGuid(), true);

            //Assert
            result.Should().Be("True - Book will be disabled");
            result.Should().BeAssignableTo<string>();

        }

        [Theory]
        public void Test_EnabledOrDisableTheBook_With_False()
        {
            //Arrange
            var users = _authorService.Setup(aut => aut.EnabledOrDisableTheBook(It.IsAny<Guid>(), It.IsAny<bool>())).Returns("False - Book will be enabled");

            //Act
            var result = _authorService.Object.EnabledOrDisableTheBook(Guid.NewGuid(), false);

            //Assert
            result.Should().Be("False - Book will be enabled");
            result.Should().BeAssignableTo<string>();

        }
    }
}
