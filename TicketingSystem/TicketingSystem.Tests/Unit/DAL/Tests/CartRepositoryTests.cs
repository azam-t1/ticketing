using TicketingSystem.DAL.Models;
using TicketingSystem.Tests.DAL.Repositories;

namespace TicketingSystem.Tests.DAL.Tests;

public class CartRepositoryTests
{
    private FakeCartRepository _repository;

    [SetUp]
    public void Setup()
    {
        _repository = new FakeCartRepository();
    }
    
    [Test]
    public async Task GetAllCartsAsync_ReturnsAllCarts()
    {
        // Arrange
        var carts = new List<Cart> { new Cart { Id = Guid.NewGuid(), CustomerId = 1 } };

        foreach (var cart in carts)
        {
            await _repository.AddAsync(cart);
        }

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(carts.Count));
    }

    [Test]
    public async Task AddAsync_AddsCart()
    {
        // Arrange
        var cart = new Cart { Id = Guid.NewGuid(), CustomerId = 1 };

        // Act
        await _repository.AddAsync(cart);
        var result = await _repository.GetCartByCustomerIdAsync(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.CustomerId, Is.EqualTo(1));
    }

    [Test]
    public async Task UpdateAsync_UpdatesCart()
    {
        // Arrange
        var cart = new Cart { Id = Guid.NewGuid(), CustomerId = 1 };
        await _repository.AddAsync(cart);
        cart.CustomerId = 2;

        // Act
        await _repository.UpdateAsync(cart);
        var result = await _repository.GetCartByCustomerIdAsync(2);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.CustomerId, Is.EqualTo(2));
    }

    [Test]
    public async Task DeleteAsync_DeletesCart()
    {
        // Arrange
        var cart = new Cart { Id = Guid.NewGuid(), CustomerId = 1 };
        await _repository.AddAsync(cart);

        // Act
        await _repository.DeleteAsync(cart.Id);
        var result = await _repository.GetCartByCustomerIdAsync(1);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task AddSeatToCartAsync_AddsSeat()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var seatId = 1;
        var priceId = 1;

        // Act
        var cart = await _repository.AddSeatToCartAsync(cartId, eventId, seatId, priceId);

        // Assert
        Assert.That(cart.CartItems.Count, Is.EqualTo(1));
        Assert.That(cart.CartItems.First().Event.Id, Is.EqualTo(eventId));
        
    }

    [Test]
    public async Task DeleteSeatFromCartAsync_DeletesSeat()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var seatId = 1;
        var priceId = 1;
        await _repository.AddSeatToCartAsync(cartId, eventId, seatId, priceId);

        // Act
        var result = await _repository.DeleteSeatFromCartAsync(cartId, eventId, seatId);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task BookCartAsync_BooksCart()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var seatId = 1;
        var priceId = 1;
        await _repository.AddSeatToCartAsync(cartId, eventId, seatId, priceId);

        // Act
        var paymentId = await _repository.BookCartAsync(cartId);

        // Assert
        Assert.That(paymentId, Is.Not.Null);
    }
}