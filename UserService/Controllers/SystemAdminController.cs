using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DigitalBooksWebAPI.Models;
using DigitalBooksWebAPI.Services;
using UserService.Implementations;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAdminController : ControllerBase
    {
        private readonly SystemAdminService systemAdminService;

        public SystemAdminController(DigitalBooksWebApiContext context)
        {
            systemAdminService = new SystemAdminService(context);
        }

        // GET: api/SystemAdmin
        [HttpGet]
        [Route("GetAllUsersReport")]
        public List<UserReport> GetAllUsersReport()
        {
            var userReport = systemAdminService.GenerateUserReport();
            return userReport;
        }

        // GET: api/SystemAdmin/5
        [HttpGet]
        [Route("GenerateSystemAdminAuthorReport")]
        public SystemAdminAuthorReport GenerateSystemAdminAuthorReport(Guid bookId)
        {
            var systemAdminAuthorReport = systemAdminService.GenerateSystemAdminAuthorReport(bookId);
            return systemAdminAuthorReport;
        }

        // PUT: api/SystemAdmin/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpGet]
        [Route("GenerateSystemAdminReaderReport")]
        public List<SystemAdminReaderReport> GenerateSystemAdminReaderReport()
        {
            var systemAdminReaderReport = systemAdminService.GenerateSystemAdminReaderReport();
            return systemAdminReaderReport;
        }

      
       
    }
}
