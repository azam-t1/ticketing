using TicketingSystem.DAL.Models;
using TicketingSystem.DAL.Repositories;

namespace TicketingSystem.Tests.DAL.Repositories;


    
public class FakeVenueRepository : IVenueRepository
{
    private readonly List<Venue> _venues = [];

    public Task<IEnumerable<Venue?>> GetAllAsync()
    {
        return Task.FromResult(_venues.AsEnumerable())!;
    }

    public Task<Venue?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_venues.FirstOrDefault(v => v.Id == id));
    }

    public Task AddAsync(Venue? entity)
    {
        if (entity != null)
        {
            _venues.Add(entity);
        }
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Venue? entity)
    {
        var existingVenue = _venues.FirstOrDefault(v => v.Id == entity?.Id);
        if (existingVenue != null && entity != null)
        {
            existingVenue.Name = entity.Name;
            existingVenue.Location = entity.Location;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        var venueToRemove = _venues.FirstOrDefault(v => v.Id == id);
        if (venueToRemove != null)
        {
            _venues.Remove(venueToRemove);
        }
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Venue>> GetVenuesByLocationAsync(string location)
    {
        return Task.FromResult(_venues.Where(v => v.Location == location).AsEnumerable());
    }

    public Task UpdateVenueAsync(Guid venueId, string name, string location)
    {
        var venue = _venues.FirstOrDefault(v => v.Id == venueId);
        if (venue != null)
        {
            venue.Name = name;
            venue.Location = location;
        }
        return Task.CompletedTask;
    }
}