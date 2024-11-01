using Microsoft.EntityFrameworkCore;
using TicketingSystem.DAL.Context;
using TicketingSystem.DAL.Models;
using TicketingSystem.DAL.Repositories.Base;

namespace TicketingSystem.DAL.Repositories;

public interface ISeatRepository : IBaseRepository<Seat>
{
    Task<IEnumerable<Seat>> GetAvailableSeatsForEventAsync(int eventId);
}

public class SeatRepository(TicketingDbContext context) : BaseRepository<Seat>(context), ISeatRepository
{
    private readonly TicketingDbContext _context = context;
    
    public async Task<IEnumerable<Seat>> GetAvailableSeatsForEventAsync(int eventId)
    {
        var bookedSeats = await _context.Tickets
            .Where(t => t.EventId == eventId)
            .Select(t => t.SeatId)
            .ToListAsync();

        return await DbSet
            .Where(s => !bookedSeats.Contains(s.Id))
            .ToListAsync();
    }
}