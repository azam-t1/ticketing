using Microsoft.EntityFrameworkCore;
using TicketingSystem.DAL.Context;
using TicketingSystem.DAL.Models;
using TicketingSystem.DAL.Repositories.Base;

namespace TicketingSystem.DAL.Repositories;

public interface ICartRepository : IBaseRepository<Cart>
{
    Task<Cart?> GetCartByCustomerIdAsync(int customerId);
    Task<Cart> AddSeatToCartAsync(Guid cartId, int eventId, int seatId, int priceId);
    Task<bool> DeleteSeatFromCartAsync(Guid cartId, int eventId, int seatId);
    Task<int?> BookCartAsync(Guid cartId);
}

public class CartRepository(TicketingDbContext context, IPaymentRepository paymentRepository) : BaseRepository<Cart>(context), ICartRepository
{
    private readonly TicketingDbContext _context = context;
    private readonly IPaymentRepository _paymentRepository = paymentRepository;
    
    public async Task<Cart?> GetCartByCustomerIdAsync(int customerId)
    {
        return await _dbSet
            .Include(c => c.CartItems)
            .ThenInclude(t => t.Seat)
            .ThenInclude(s => s.Row)
            .ThenInclude(r => r.Section)
            .ThenInclude(s => s.Manifest)
            .ThenInclude(m => m.Venue)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);
    }

    public async Task<Cart> AddSeatToCartAsync(Guid cartId, int eventId, int seatId, int priceId)
    {
        var cart = await GetByIdAsync(cartId);
        if (cart == null)
        {
            cart = new Cart
            {
                CustomerId = 1, // hardcoded customer id until authentication is implemented
                CartItems = new List<CartItem>()
                {
                    new CartItem()
                    {
                        EventId = eventId,
                        SeatId = seatId,
                        PriceId = priceId
                    }
                }
            };
            
            await AddAsync(cart);
        }
        else
        {
            var cartItem = new CartItem
            {
                EventId = eventId,
                SeatId = seatId,
                PriceId = priceId
            };

            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
        }

        return cart;
    }

    public async Task<bool> DeleteSeatFromCartAsync(Guid cartId, int eventId, int seatId)
    {
        var cart = await GetByIdAsync(cartId);

        var cartItem = cart?.CartItems.FirstOrDefault(ci => ci.EventId == eventId && ci.SeatId == seatId);
        if (cartItem == null)
        {
            return false;
        }

        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int?> BookCartAsync(Guid cartId)
    {
        var cart = await GetByIdAsync(cartId);
        if (cart == null || !cart.CartItems.Any())
        {
            return null;
        }

        foreach (var cartItem in cart.CartItems)
        {
            cartItem.Seat.Status = SeatStatus.Booked;
        }

        // Generate a new Payment
        var paymentAmount = cart.CartItems.Sum(ci => ci.Price.Amount);
        var payment = await _paymentRepository.CreatePaymentAsync(cart.Id, paymentAmount);
        
        await _context.SaveChangesAsync();
        return payment.Id;
    }
}