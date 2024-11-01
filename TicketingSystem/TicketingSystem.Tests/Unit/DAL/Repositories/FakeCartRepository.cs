using TicketingSystem.DAL.Models;
using TicketingSystem.DAL.Repositories;

namespace TicketingSystem.Tests.DAL.Repositories;

public class FakeCartRepository : ICartRepository
{
    private readonly FakePaymentRepository _fakePaymentRepository = new();
    private readonly List<Cart> _carts = new();

    public Task<IEnumerable<Cart?>> GetAllAsync()
    {
        return Task.FromResult(_carts.AsEnumerable())!;
    }

    public Task<Cart?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_carts.FirstOrDefault(c => c.Id == id));
    }

    public Task AddAsync(Cart entity)
    {
        _carts.Add(entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Cart entity)
    {
        var existingCart = _carts.FirstOrDefault(c => c.Id == entity.Id);
        if (existingCart != null)
        {
            existingCart.CustomerId = entity.CustomerId;
            existingCart.CartItems = entity.CartItems;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        var cartToRemove = _carts.FirstOrDefault(c => c.Id == id);
        if (cartToRemove != null)
        {
            _carts.Remove(cartToRemove);
        }
        return Task.CompletedTask;
    }

    public Task<Cart?> GetCartByCustomerIdAsync(int customerId)
    {
        return Task.FromResult(_carts.FirstOrDefault(c => c.CustomerId == customerId));
    }

    public Task<Cart> AddSeatToCartAsync(Guid cartId, Guid eventId, int seatId, int priceId)
    {
        var cart = _carts.FirstOrDefault(c => c.Id == cartId);
        if (cart == null)
        {
            cart = new Cart
            {
                Id = cartId,
                CustomerId = 1, // hardcoded customer id until authentication is implemented
                CartItems = new List<CartItem>
                {
                    new CartItem
                    {
                        EventId = eventId,
                        Event = new Event() { Id = eventId, Name = "Event1" },
                        SeatId = seatId,
                        Seat = new Seat { Id = seatId, Status = SeatStatus.Available },
                        PriceId = priceId,
                        Price = new Price { Id = priceId, Amount = 100 }
                    }
                }
            };
            _carts.Add(cart);
        }
        else
        {
            cart.CartItems.Add(new CartItem
            {
                EventId = eventId,
                SeatId = seatId,
                PriceId = priceId
            });
        }
        return Task.FromResult(cart);
    }

    public Task<bool> DeleteSeatFromCartAsync(Guid cartId, Guid eventId, int seatId)
    {
        var cart = _carts.FirstOrDefault(c => c.Id == cartId);
        if (cart == null)
            return Task.FromResult(false);

        var cartItem = cart?.CartItems.FirstOrDefault(ci => ci.EventId == eventId && ci.SeatId == seatId);
        if (cartItem == null)
            return Task.FromResult(false);
        
        cart?.CartItems.Remove(cartItem);
        return Task.FromResult(true);
    }

    public async Task<Guid?> BookCartAsync(Guid cartId)
    {
        var cart = _carts.FirstOrDefault(c => c.Id == cartId);
        
        if (cart == null || !cart.CartItems.Any())
            return null;

        foreach (var cartItem in cart.CartItems)
            cartItem.Seat.Status = SeatStatus.Booked;
        
        // Simulate payment creation
        var payment = await _fakePaymentRepository.CreatePaymentAsync(cart.Id, cart.CartItems.Sum(ci => ci.Price.Amount));
        return payment.Id;        
    }
}