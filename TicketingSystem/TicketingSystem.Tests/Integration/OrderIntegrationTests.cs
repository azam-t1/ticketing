// TicketingSystem.IntegrationTests/IntegrationTests/CartIntegrationTests.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using TicketingSystem.API.Controllers;
using TicketingSystem.DAL.Context;
using TicketingSystem.DAL.Models;
using TicketingSystem.DAL.Repositories;

namespace TicketingSystem.Tests.Integration
{
    public class OrderIntegrationTests
    {
        private Mock<TicketingDbContext> _mockContext;
        private Mock<DbSet<Cart>> _mockCartSet;
        private Mock<DbSet<Seat>> _mockSeatSet;
        private OrdersController _controller;
        private ICartRepository _cartRepository;
        private ISeatRepository _seatRepository;
        private IPaymentRepository _paymentRepository;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<TicketingDbContext>();
            _mockCartSet = new Mock<DbSet<Cart>>();
            _mockSeatSet = new Mock<DbSet<Seat>>();
            _mockContext.Setup(m => m.Carts).Returns(_mockCartSet.Object);
            _mockContext.Setup(m => m.Seats).Returns(_mockSeatSet.Object);
            
            _paymentRepository = new PaymentRepository(_mockContext.Object);
            _cartRepository = new CartRepository(_mockContext.Object, _paymentRepository);
            _seatRepository = new SeatRepository(_mockContext.Object);
            _controller = new OrdersController(_cartRepository);
        }
        
    }
}