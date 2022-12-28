using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DigitalBooksWebAPI.Models;
using UserService.Implementations;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly AuthorService authorService;

        public AuthorController(DigitalBooksWebApiContext context)
        {
            authorService = new AuthorService(context); 
        }

        [HttpGet]
        [Route("GetAllBooks")]
        public List<Book> GetAuthorBooks(Guid autorId)
        {
            var authorBooks = authorService.GetAuthorBooks(autorId);
            return authorBooks;
        }

        [HttpPost]
        [Route("CreateBook")]
        public string CreateBook(Book book)
        {
            var createMessage = authorService.CreateBook(book);
            return createMessage;
        }

        [HttpGet]
        [Route("EnabledOrDisableTheBook")]
        public string EnabledOrDisableTheBook(Guid bookId, bool isDisabled)
        {
            var messageToReader = authorService.EnabledOrDisableTheBook(bookId, isDisabled);
            return messageToReader;
        }
    }
}
