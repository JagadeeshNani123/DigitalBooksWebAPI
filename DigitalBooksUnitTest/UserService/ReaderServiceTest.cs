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
    public class ReaderServiceTest
    {
        private readonly Mock<IReaderService> _readerService;
        public ReaderServiceTest()
        {
            _readerService = new Mock<IReaderService>();
        }

        [Theory]
        public void Test_GetAllSubscriptions()
        {
            //Arrange
            _readerService.Setup(aut => aut.GetAllSubscriptions(It.IsAny<Guid>())).Returns(new List<SubscriptionReport>());

            //Act
            var result = _readerService.Object.GetAllSubscriptions(Guid.NewGuid());

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<SubscriptionReport>>();
        }

        [Theory]
        public void Test_CancelTheSubscriptionWithin24Hrs()
        {
            //Arrange
            _readerService.Setup(aut => aut.CancelTheSubscriptionWithin24Hrs(It.IsAny<Guid>())).Returns("Cancelled Subscription");

            //Act
            var result = _readerService.Object.CancelTheSubscriptionWithin24Hrs(Guid.NewGuid());

            //Assert
            result.Should().Be("Cancelled Subscription");
            result.Should().BeAssignableTo<string>();
        }

        [Theory]
        public void Test_GenerateSubscriptionInvoices()
        {
            //Arrange
            var subscriptionInvoiceMock = new Mock<SubscriptionInvoice>();
            _readerService.Setup(aut => aut.GenerateSubscriptionInvoice(It.IsAny<Guid>())).Returns(subscriptionInvoiceMock.Object);

            //Act
            var result = _readerService.Object.GenerateSubscriptionInvoice(Guid.NewGuid());

            //Assert
            result.Should().Be(subscriptionInvoiceMock.Object);
            result.Should().BeAssignableTo<SubscriptionInvoice>();
        }

        [Theory]
        public void Test_ReadTheBook()
        {
            //Arrange
            _readerService.Setup(aut => aut.ReadTheBook(It.IsAny<Guid>())).Returns("Test book content");

            //Act
            var result = _readerService.Object.ReadTheBook(Guid.NewGuid());

            //Assert
            result.Should().Be("Test book content");
            result.Should().BeAssignableTo<string>();
        }

        [Theory]
        public void Test_Subscribe()
        {
            //Arrange
            _readerService.Setup(aut => aut.Subscribe(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns("Successfully subscribed");

            //Act
            var result = _readerService.Object.Subscribe(Guid.NewGuid(), Guid.NewGuid());

            //Assert
            result.Should().Be("Successfully subscribed");
            result.Should().BeAssignableTo<string>();
        }
    }
}
