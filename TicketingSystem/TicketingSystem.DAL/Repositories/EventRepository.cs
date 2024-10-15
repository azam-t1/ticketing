using Microsoft.EntityFrameworkCore;
using TicketingSystem.DAL.Context;
using TicketingSystem.DAL.Models;
using TicketingSystem.DAL.Repositories.Base;

namespace TicketingSystem.DAL.Repositories;

public interface IEventRepository : IBaseRepository<Event>
{
    Task<IEnumerable<Event>> GetEventsByVenueAsync(int venueId);
    Task<IEnumerable<Event>> SearchEventsAsync(string searchTerm, DateTime? startDate = null, DateTime? endDate = null);
}

public class EventRepository(TicketingDbContext context) : BaseRepository<Event>(context), IEventRepository
{
    private readonly TicketingDbContext _context = context;
    
    public async Task<IEnumerable<Event>> GetEventsByVenueAsync(int venueId)
    {
        return await _dbSet.Where(e => e.VenueId == venueId).ToListAsync();
    }

    public async Task<IEnumerable<Event>> SearchEventsAsync(string searchTerm, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(e => e.Name.Contains(searchTerm));
        }

        if (startDate.HasValue)
        {
            query = query.Where(e => e.Date >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(e => e.Date <= endDate.Value);
        }

        return await query.ToListAsync();
    }

}