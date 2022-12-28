using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DigitalBooksWebAPI.Models;
using System.Threading.Channels;
using DigitalBooksWebAPI.Services;
using UserService.Implementations;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReaderController : ControllerBase
    {
        private readonly ReaderService readerService;

        public ReaderController(DigitalBooksWebApiContext context)
        {
            readerService = new ReaderService(context);
        }

        // GET: api/Reader
        [HttpGet]
        [Route("GetAllSubscriptions")]
        public List<SubscriptionReport> GetAllSubscriptions(Guid userId)
        {
            return readerService.GetAllSubscriptions(userId);
        }

        // GET: api/Reader/5
        [HttpDelete]
        [Route("CancelTheSubscription/subscriptionId")]
        public string CancelTheSubscriptionWithin24Hrs(Guid subscriptionId)
        {
            return readerService.CancelTheSubscriptionWithin24Hrs(subscriptionId);
        }

        // PUT: api/Reader/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpGet]
        [Route("ReadBook")]
        public string ReadTheBook(Guid bookId)
        {
            return readerService.ReadTheBook(bookId);
        }

        [HttpGet]
        [Route("Subscribe")]
        public string Subscribe( Guid userId, Guid bookId)
        {
            var message = readerService.Subscribe(userId, bookId);
            return message;
        }


        [HttpGet]
        [Route("GenerateSubscriptionInvoice")]
        public SubscriptionInvoice GenerateSubscriptionInvoice(Guid subscriptionId)
        {
            var invoice = readerService.GenerateSubscriptionInvoice(subscriptionId);
            return invoice;
        }

    }
}
