using Microsoft.AspNetCore.Mvc;
using Moq;
using TicketingSystem.API.Controllers;
using TicketingSystem.DAL.Models;
using TicketingSystem.DAL.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketingSystem.Tests.API
{
    public class CartsControllerTests
    {
        private Mock<ICartRepository> _mockCartRepository;
        private OrdersController _controller;

        [SetUp]
        public void Setup()
        {
            _mockCartRepository = new Mock<ICartRepository>();
            _controller = new OrdersController(_mockCartRepository.Object);
        }

        [Test]
        public async Task GetCart_ReturnsNotFound_WhenCartDoesNotExist()
        {
            // Arrange
            _mockCartRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Cart)null);

            // Act
            var result = await _controller.GetCart(Guid.NewGuid());

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetCart_ReturnsOkResult_WithCart()
        {
            // Arrange
            var cart = new Cart { Id = Guid.NewGuid(), Customer = new Customer()
            {
                Id = 1,
                Name = "User 1" 
            }};
            
            _mockCartRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(cart);

            // Act
            var result = await _controller.GetCart(cart.Id);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(cart));
        }
    }
}