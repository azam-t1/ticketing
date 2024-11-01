using Microsoft.EntityFrameworkCore;
using TicketingSystem.DAL.Context;
using TicketingSystem.DAL.Models;
using TicketingSystem.DAL.Repositories.Base;

namespace TicketingSystem.DAL.Repositories;

public interface IVenueRepository : IBaseRepository<Venue>
{
    Task<IEnumerable<Venue>> GetVenuesByLocationAsync(string location);
    Task UpdateVenueAsync(Guid venueId, string name, string location);
}

public class VenueRepository(TicketingDbContext context) : BaseRepository<Venue>(context), IVenueRepository
{
    private readonly TicketingDbContext _context = context;

    public async Task<IEnumerable<Venue>> GetVenuesByLocationAsync(string location)
    {
        return await DbSet.Where(v => v.Location == location).ToListAsync();
    }

    public async Task UpdateVenueAsync(Guid venueId, string name, string location)
    {
        var venue = await DbSet.FindAsync(venueId);
        if (venue != null)
        {
            venue.Name = name;
            venue.Location = location;
            await _context.SaveChangesAsync();
        }
    }
}