// TicketingSystem.IntegrationTests/IntegrationTests/CartIntegrationTests.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.API.Controllers;
using TicketingSystem.DAL.Context;
using TicketingSystem.DAL.Models;
using TicketingSystem.DAL.Repositories;

namespace TicketingSystem.Tests.Integration
{
    public class OrderIntegrationTests
    {
        private TicketingDbContext _context;
        private OrdersController _ordersController;
        private PaymentsController _paymentsController;
        private ICartRepository _cartRepository;
        private IPaymentRepository _paymentRepository;
        
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TicketingDbContext>()
                .UseInMemoryDatabase(databaseName: "TicketingSystem")
                .Options;

            _context = new TicketingDbContext(options);
            _context.Database.EnsureCreated();

            _paymentRepository = new PaymentRepository(_context);
            _cartRepository = new CartRepository(_context, _paymentRepository);

            _ordersController = new OrdersController(_cartRepository);
            _paymentsController = new PaymentsController(_paymentRepository);
        }
        
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }


        [Test]
        public async Task CreateOrder_ShouldReturnOkResult()
        {
            // Arrange
            var cart = new Cart(){ Id = Guid.NewGuid(), CustomerId = 1};
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            
            // Act
            var result = await _ordersController.BookCart(cart.Id);
            
            // Assert
            Assert.That(result, Is.InstanceOf<OkResult>());
        }
        
        [Test]
        public async Task CompletePayment_ShouldReturnOkResult()
        {
            // Arrange
            var cart = new Cart()
            {
                Id = Guid.NewGuid(), 
                CustomerId = 1,
                Customer = new Customer(){ Id = 1, Name = "Test Customer"},
                CartItems = new List<CartItem>()
                {
                    new CartItem()
                    {
                        Id = 1,
                        EventId = Guid.NewGuid(),
                        Event = new Event()
                        {
                            Id = Guid.NewGuid(),
                            Name = "Test Event",
                            Offers = new List<Offer>()
                            {
                                new Offer()
                                {
                                    Id = 1,
                                    EventId = 1,
                                }
                            }
                        },
                        SeatId = 1,
                        Seat =
                        {
                            Id = 1,
                            Price = new Price()
                            {
                                Id = 1,
                                Amount = 100,
                                Offer = new Offer()
                                {
                                    Id = 1,
                                    EventId = 1
                                }
                            }
                        },
                        PriceId = 1,
                        Price = new Price()
                        {
                            Id = 1,
                            Amount = 100,
                            Offer = new Offer()
                            {
                                Id = 1,
                                EventId = 1
                            }
                        }
                    }
                }
            };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            
            // Act
            var result = await _paymentsController.CompletePayment(cart.Id);
            
            // Assert
            Assert.That(result, Is.InstanceOf<OkResult>());
            
            var updatedPayment = await _paymentRepository.GetByIdAsync(cart.Id);
            Assert.That(updatedPayment?.PaymentStatus, Is.EqualTo(PaymentStatus.Completed));
        }
        
        [Test]
        public async Task FailPayment_ShouldReturnOkResult()
        {
            // Arrange
            var cart = new Cart(){ Id = Guid.NewGuid(), CustomerId = 1};
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            
            // Act
            var result = await _paymentsController.FailPayment(cart.Id);
            
            // Assert
            Assert.That(result, Is.InstanceOf<OkResult>());
            
            var updatedPayment = await _paymentRepository.GetByIdAsync(cart.Id);
            Assert.That(updatedPayment?.PaymentStatus, Is.EqualTo(PaymentStatus.Failed));
        }
    }
}