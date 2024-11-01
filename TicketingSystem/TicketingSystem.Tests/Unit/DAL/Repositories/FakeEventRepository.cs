using TicketingSystem.DAL.Models;
using TicketingSystem.DAL.Repositories;

namespace TicketingSystem.Tests.DAL.Repositories;

public class FakeEventRepository : IEventRepository
{
    private readonly List<Event> _events = [];

    public Task<IEnumerable<Event?>> GetAllAsync()
    {
        return Task.FromResult(_events.AsEnumerable())!;
    }

    public Task<Event?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_events.FirstOrDefault(e => e.Id == id));
    }

    public Task AddAsync(Event? entity)
    {
        if (entity != null)
        {
            _events.Add(entity);
        }
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Event? entity)
    {
        var existingEvent = _events.FirstOrDefault(e => e.Id == entity?.Id);
        if (existingEvent != null && entity != null)
        {
            existingEvent.Name = entity.Name;
            existingEvent.VenueId = entity.VenueId;
            existingEvent.Date = entity.Date;
            existingEvent.Time = entity.Time;
            existingEvent.Offers = entity.Offers;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        var eventToRemove = _events.FirstOrDefault(e => e.Id == id);
        if (eventToRemove != null)
        {
            _events.Remove(eventToRemove);
        }
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Event>> GetEventsByVenueAsync(int venueId)
    {
        return Task.FromResult(_events.Where(e => e.VenueId == venueId).AsEnumerable());
    }

    public Task<IEnumerable<Event>> SearchEventsAsync(string searchTerm, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _events.AsQueryable();

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

        return Task.FromResult(query.AsEnumerable());
    }
}