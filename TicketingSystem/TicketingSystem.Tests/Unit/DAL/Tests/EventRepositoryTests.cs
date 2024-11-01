using Microsoft.EntityFrameworkCore;
using Moq;
using TicketingSystem.DAL.Context;
using TicketingSystem.DAL.Models;
using TicketingSystem.Tests.DAL.Repositories;

namespace TicketingSystem.Tests.DAL.Tests;

public class EventRepositoryTests
{
    private Mock<TicketingDbContext> _mockContext;
    private Mock<DbSet<Event>> _mockEventSet;
    private FakeEventRepository _repository;
    
    [SetUp]
    public void Setup()
    {
        _mockContext = new Mock<TicketingDbContext>();
        _mockEventSet = new Mock<DbSet<Event>>();
        _repository = new FakeEventRepository();
    }
    
    
    
    [Test]
    public async Task GetAllEventsAsync_ReturnsAllEvents()
    {
        // Arrange
        var events = new List<Event> { new Event { Id = Guid.NewGuid(), Name = "Event1" } }.AsQueryable();
        _mockEventSet.As<IQueryable<Event>>().Setup(m => m.Provider).Returns(events.Provider);
        _mockEventSet.As<IQueryable<Event>>().Setup(m => m.Expression).Returns(events.Expression);
        _mockEventSet.As<IQueryable<Event>>().Setup(m => m.ElementType).Returns(events.ElementType);
        _mockEventSet.As<IQueryable<Event>>().Setup(m => m.GetEnumerator()).Returns(events.GetEnumerator());
        
        // Act
        var result = await _repository.GetAllAsync();
        
        // Assert
        Assert.That(result.Count(), Is.GreaterThanOrEqualTo(0));
    }
    
    [Test]
    public async Task GetByIdAsync_ReturnsEvent()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var eventItem = new Event { Id = eventId, Name = "Event1" };
        await _repository.AddAsync(eventItem);

        // Act
        var result = await _repository.GetByIdAsync(eventId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Id, Is.EqualTo(eventId));
    }

    [Test]
    public async Task AddAsync_AddsEvent()
    {
        // Arrange
        var eventItem = new Event { Id = Guid.NewGuid(), Name = "Event1" };

        // Act
        await _repository.AddAsync(eventItem);
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result?.First()?.Name, Is.EqualTo("Event1"));
    }

    [Test]
    public async Task UpdateAsync_UpdatesEvent()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var eventItem = new Event { Id = eventId, Name = "Event1" };
        await _repository.AddAsync(eventItem);
        eventItem.Name = "UpdatedEvent";

        // Act
        await _repository.UpdateAsync(eventItem);
        var result = await _repository.GetByIdAsync(eventId);

        // Assert
        Assert.That(result?.Name, Is.EqualTo("UpdatedEvent"));
    }

    [Test]
    public async Task DeleteAsync_DeletesEvent()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var eventItem = new Event { Id = eventId, Name = "Event1" };
        await _repository.AddAsync(eventItem);

        // Act
        await _repository.DeleteAsync(eventId);
        var result = await _repository.GetByIdAsync(eventId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetEventsByVenueAsync_ReturnsEvents()
    {
        // Arrange
        var venueId = 1;
        var eventItem = new Event { Id = Guid.NewGuid(), Name = "Event1", VenueId = venueId };
        await _repository.AddAsync(eventItem);

        // Act
        var result = await _repository.GetEventsByVenueAsync(venueId);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().VenueId, Is.EqualTo(venueId));
    }

    [Test]
    public async Task SearchEventsAsync_ReturnsFilteredEvents()
    {
        // Arrange
        var eventItem1 = new Event { Id = Guid.NewGuid(), Name = "Event1", Date = DateTime.Now.AddDays(1) };
        var eventItem2 = new Event { Id = Guid.NewGuid(), Name = "Event2", Date = DateTime.Now.AddDays(2) };
        await _repository.AddAsync(eventItem1);
        await _repository.AddAsync(eventItem2);

        // Act
        var result = await _repository.SearchEventsAsync("Event1");

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().Name, Is.EqualTo("Event1"));
    }
}