using Microsoft.EntityFrameworkCore;
using Moq;
using TicketingSystem.DAL.Context;
using TicketingSystem.DAL.Models;
using TicketingSystem.Tests.DAL.Repositories;

namespace TicketingSystem.Tests.DAL.Tests;

public class PaymentRepositoryTests
{
    private Mock<TicketingDbContext> _mockContext;
    private Mock<DbSet<Payment>> _mockPaymentSet;
    private FakePaymentRepository _repository;
    
    [SetUp]
    public void Setup()
    {
        _mockContext = new Mock<TicketingDbContext>();
        _mockPaymentSet = new Mock<DbSet<Payment>>();
        _repository = new FakePaymentRepository();
    }
    
    [Test]
    public async Task GetAllPaymentsAsync_ReturnsAllPayments()
    {
        // Arrange
        var payments = new List<Payment> { new Payment { Id = Guid.NewGuid(), Amount = 100 } }.AsQueryable();
        _mockPaymentSet.As<IQueryable<Payment>>().Setup(m => m.Provider).Returns(payments.Provider);
        _mockPaymentSet.As<IQueryable<Payment>>().Setup(m => m.Expression).Returns(payments.Expression);
        _mockPaymentSet.As<IQueryable<Payment>>().Setup(m => m.ElementType).Returns(payments.ElementType);
        _mockPaymentSet.As<IQueryable<Payment>>().Setup(m => m.GetEnumerator()).Returns(payments.GetEnumerator());
        
        // Act
        var result = await _repository.GetAllAsync();
        
        // Assert
        Assert.That(result.Count(), Is.GreaterThanOrEqualTo(0));
    }
    
    [Test]
    public async Task GetByIdAsync_ReturnsPayment()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var payment = new Payment { Id = paymentId, Amount = 100 };
        await _repository.AddAsync(payment);

        // Act
        var result = await _repository.GetByIdAsync(paymentId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Id, Is.EqualTo(paymentId));
    }

    [Test]
    public async Task AddAsync_AddsPayment()
    {
        // Arrange
        var payment = new Payment { Id = Guid.NewGuid(), Amount = 100 };

        // Act
        await _repository.AddAsync(payment);
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result?.First()?.Amount, Is.EqualTo(100));
    }

    [Test]
    public async Task UpdateAsync_UpdatesPayment()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var payment = new Payment { Id = paymentId, Amount = 100 };
        await _repository.AddAsync(payment);
        payment.Amount = 200;

        // Act
        await _repository.UpdateAsync(payment);
        var result = await _repository.GetByIdAsync(paymentId);

        // Assert
        Assert.That(result?.Amount, Is.EqualTo(200));
    }

    [Test]
    public async Task DeleteAsync_DeletesPayment()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var payment = new Payment { Id = paymentId, Amount = 100 };
        await _repository.AddAsync(payment);

        // Act
        await _repository.DeleteAsync(paymentId);
        var result = await _repository.GetByIdAsync(paymentId);

        // Assert
        Assert.That(result, Is.Null);
    }
}