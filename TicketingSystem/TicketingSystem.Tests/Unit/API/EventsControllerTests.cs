using Microsoft.AspNetCore.Mvc;
using Moq;
using TicketingSystem.API.Controllers;
using TicketingSystem.DAL.Models;
using TicketingSystem.DAL.Repositories;

namespace TicketingSystem.Tests.API
{
    public class EventsControllerTests
    {
        private Mock<IEventRepository> _mockEventRepository;
        private EventsController _controller;

        [SetUp]
        public void Setup()
        {
            _mockEventRepository = new Mock<IEventRepository>();
            _controller = new EventsController(_mockEventRepository.Object);
        }

        [Test]
        public async Task GetEvents_ReturnsOkResult_WithListOfEvents()
        {
            // Arrange
            var events = new List<Event> { new Event { Id = Guid.NewGuid(), Name = "Event 1" } };
            _mockEventRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(events);

            // Act
            var result = await _controller.GetEvents();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(events));
        }

        [Test]
        public async Task GetSeats_ReturnsNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            _mockEventRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Event)null);

            // Act
            var result = await _controller.GetSeats(Guid.NewGuid(), 1);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetSeats_ReturnsNotFound_WhenSectionDoesNotExist()
        {
            // Arrange
            var eventEntity = new Event
            {
                Id = Guid.NewGuid(),
                Venue = new Venue { Sections = new List<Section>() }
            };
            _mockEventRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(eventEntity);

            // Act
            var result = await _controller.GetSeats(eventEntity.Id, 1);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetSeats_ReturnsOkResult_WithListOfSeats()
        {
            // Arrange
            var eventEntity = new Event
            {
                Id = Guid.NewGuid(),
                Venue = new Venue
                {
                    Sections = new List<Section>
                    {
                        new Section
                        {
                            Id = 1,
                            Seats = new List<Seat>
                            {
                                new Seat
                                {
                                    Id = 1,
                                    Row = new Row { SectionId = 1 },
                                    RowId = 1,
                                    Status = SeatStatus.Available,
                                    Price = new Price { Id = 1, PriceLevel = "Standard", Amount = 100 }
                                }
                            }
                        }
                    }
                }
            };
            _mockEventRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(eventEntity);

            // Act
            var result = await _controller.GetSeats(eventEntity.Id, 1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var seats = okResult.Value as IEnumerable<object>;
            Assert.That(seats, Is.Not.Null);
            Assert.That(seats.Count(), Is.EqualTo(1));
        }
    }
}