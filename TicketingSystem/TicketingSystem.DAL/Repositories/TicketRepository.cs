using Microsoft.EntityFrameworkCore;
using TicketingSystem.DAL.Context;
using TicketingSystem.DAL.Models;
using TicketingSystem.DAL.Repositories.Base;

namespace TicketingSystem.DAL.Repositories;

public interface ITicketRepository : IBaseRepository<Ticket>
{
    Task<bool> CancelTicketAsync(int id);
    Task<IEnumerable<Ticket>> GetTicketsByCustomerIdAsync(int customerId);
}

public class TicketRepository(TicketingDbContext context) : BaseRepository<Ticket>(context), ITicketRepository
{
    private readonly TicketingDbContext _context = context;

    public async Task<bool> CancelTicketAsync(int ticketId)
    {
        var ticket = await _dbSet.FindAsync(ticketId);
        if (ticket != null)
        {
            // Perform any additional logic for canceling the ticket
            // For example, update the ticket status, create a refund, etc.
            
            _dbSet.Remove(ticket);
            await _context.SaveChangesAsync();
        }

        return true;
    }
    
    public async Task<IEnumerable<Ticket>> GetTicketsByCustomerIdAsync(int customerId)
    {
        return await _dbSet
            .Where(t => t.CustomerId == customerId)
            .ToListAsync();
    }
}