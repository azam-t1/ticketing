using Microsoft.EntityFrameworkCore;
using TicketingSystem.DAL.Context;
using TicketingSystem.DAL.Models;
using TicketingSystem.DAL.Repositories.Base;

namespace TicketingSystem.DAL.Repositories;

public interface ICartRepository : IBaseRepository<Cart>
{
    Task<Cart?> GetCartByCustomerIdAsync(int customerId);
    Task AddTicketToCartAsync(int customerId, int ticketId);
    Task RemoveTicketFromCartAsync(int customerId, int ticketId);
}

public class CartRepository(TicketingDbContext context) : BaseRepository<Cart>(context), ICartRepository
{
    private readonly TicketingDbContext _context = context;
    
    public async Task<Cart?> GetCartByCustomerIdAsync(int customerId)
    {
        return await _dbSet
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Ticket)
            .ThenInclude(t => t.Seat)
            .ThenInclude(s => s.Row)
            .ThenInclude(r => r.Section)
            .ThenInclude(s => s.Manifest)
            .ThenInclude(m => m.Venue)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);
    }

    public async Task AddTicketToCartAsync(int customerId, int ticketId)
    {
        var cart = await GetCartByCustomerIdAsync(customerId) ?? new Cart { CustomerId = customerId };

        var ticket = await _context.Tickets
            .Include(t => t.Seat)
            .FirstOrDefaultAsync(t => t.Id == ticketId);

        if (ticket != null)
        {
            var cartItem = new CartItem
            {
                CartId = cart.Id,
                TicketId = ticketId,
                Ticket = ticket
            };

            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveTicketFromCartAsync(int customerId, int ticketId)
    {
        var cart = await GetCartByCustomerIdAsync(customerId);

        if (cart != null)
        {
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.TicketId == ticketId);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}