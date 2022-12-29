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
using UserService.Implementations;
using UserService.Services;

namespace DigitalBooksUnitTest.UserService
{
    
    public class SystemAdminServiceTest
    {
        private readonly Mock<ISystemAdminService> _sysAdminService;
        public SystemAdminServiceTest()
        {
            _sysAdminService= new Mock<ISystemAdminService>();
        }
        [Theory]
        public void Test_GenerateSystemAdminAuthorReport()
        {
            //Arrange
            var sysAdminAutReport = new Mock<SystemAdminAuthorReport>();
            _sysAdminService.Setup(aut => aut.GenerateSystemAdminAuthorReport(It.IsAny<Guid>())).Returns(sysAdminAutReport.Object);

            //Act
            var result = _sysAdminService.Object.GenerateSystemAdminAuthorReport(Guid.NewGuid());

            //Assert
            result.Should().Be(sysAdminAutReport.Object);
            result.Should().BeAssignableTo<SystemAdminAuthorReport>();

        }

        [Theory]
        public void Test_GenerateSystemAdminReaderReport()
        {
            //Arrange
             _sysAdminService.Setup(aut => aut.GenerateSystemAdminReaderReport()).Returns(new List<SystemAdminReaderReport>());

            //Act
            var result = _sysAdminService.Object.GenerateSystemAdminReaderReport();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<SystemAdminReaderReport>>();

        }

        [Theory]
        public void Test_GenerateUserReport()
        {
            //Arrange
            var users = _sysAdminService.Setup(aut => aut.GenerateUserReport()).Returns(new List<UserReport>());

            //Act
            var result = _sysAdminService.Object.GenerateUserReport();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<UserReport>>();

        }

        [Theory]
        public void Test_GetSubscribedReaders()
        {
            //Arrange
             _sysAdminService.Setup(aut => aut.GetSubscribedReaders(It.IsAny<Guid>())).Returns(new List<User>());

            //Act
            var result = _sysAdminService.Object.GetSubscribedReaders(Guid.NewGuid());

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<User>>();

        }

        [Theory]
        public void Test_GetSubscriptionsByBookId()
        {
            //Arrange
            _sysAdminService.Setup(aut => aut.GetSubscriptionsByBookId(It.IsAny<Guid>())).Returns(new List<Subscription>());

            //Act
            var result = _sysAdminService.Object.GetSubscriptionsByBookId(Guid.NewGuid());

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<Subscription>>();

        }
    }
}
