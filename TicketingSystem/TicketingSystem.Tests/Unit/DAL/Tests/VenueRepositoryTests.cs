using TicketingSystem.DAL.Models;
using TicketingSystem.Tests.DAL.Repositories;

namespace TicketingSystem.Tests.DAL.Tests;


public class VenueRepositoryTests
{
    private FakeVenueRepository _repository;

    [SetUp]
    public void Setup()
    {
        _repository = new FakeVenueRepository();
    }

    [Test]
    public async Task GetAllVenuesAsync_ReturnsAllVenues()
    {
        // Arrange
        var venues = new List<Venue> { new Venue { Id = Guid.NewGuid(), Name = "Venue1", Location = "Location1" } };

        foreach (var venue in venues)
        {
            await _repository.AddAsync(venue);
        }

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(venues.Count));
    }

    [Test]
    public async Task GetByIdAsync_ReturnsVenue()
    {
        // Arrange
        var venueId = Guid.NewGuid();
        var venue = new Venue { Id = venueId, Name = "Venue1", Location = "Location1" };
        await _repository.AddAsync(venue);

        // Act
        var result = await _repository.GetByIdAsync(venueId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Id, Is.EqualTo(venueId));
    }

    [Test]
    public async Task AddAsync_AddsVenue()
    {
        // Arrange
        var venue = new Venue { Id = Guid.NewGuid(), Name = "Venue1", Location = "Location1" };

        // Act
        await _repository.AddAsync(venue);
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result?.First()?.Name, Is.EqualTo("Venue1"));
    }

    [Test]
    public async Task UpdateAsync_UpdatesVenue()
    {
        // Arrange
        var venueId = Guid.NewGuid();
        var venue = new Venue { Id = venueId, Name = "Venue1", Location = "Location1" };
        await _repository.AddAsync(venue);
        venue.Name = "UpdatedVenue";

        // Act
        await _repository.UpdateAsync(venue);
        var result = await _repository.GetByIdAsync(venueId);

        // Assert
        Assert.That(result?.Name, Is.EqualTo("UpdatedVenue"));
    }

    [Test]
    public async Task DeleteAsync_DeletesVenue()
    {
        // Arrange
        var venueId = Guid.NewGuid();
        var venue = new Venue { Id = venueId, Name = "Venue1", Location = "Location1" };
        await _repository.AddAsync(venue);

        // Act
        await _repository.DeleteAsync(venueId);
        var result = await _repository.GetByIdAsync(venueId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetVenuesByLocationAsync_ReturnsVenues()
    {
        // Arrange
        var location = "Location1";
        var venue = new Venue { Id = Guid.NewGuid(), Name = "Venue1", Location = location };
        await _repository.AddAsync(venue);

        // Act
        var result = await _repository.GetVenuesByLocationAsync(location);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().Location, Is.EqualTo(location));
    }
}