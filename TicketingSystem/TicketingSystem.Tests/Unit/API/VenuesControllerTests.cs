using Microsoft.AspNetCore.Mvc;
using Moq;
using TicketingSystem.API.Controllers;
using TicketingSystem.DAL.Models;
using TicketingSystem.DAL.Repositories;

namespace TicketingSystem.Tests.API
{
    public class VenuesControllerTests
    {
        private Mock<IVenueRepository> _mockVenueRepository;
        private VenuesController _controller;

        [SetUp]
        public void Setup()
        {
            _mockVenueRepository = new Mock<IVenueRepository>();
            _controller = new VenuesController(_mockVenueRepository.Object);
        }

        [Test]
        public async Task GetVenues_ReturnsOkResult_WithListOfVenues()
        {
            // Arrange
            var venues = new List<Venue> { new Venue { Id = Guid.NewGuid(), Name = "Venue 1" } };
            _mockVenueRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(venues);

            // Act
            var result = await _controller.GetVenues();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(venues));
        }

        [Test]
        public async Task GetVenue_ReturnsNotFound_WhenVenueDoesNotExist()
        {
            // Arrange
            _mockVenueRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Venue)null);

            // Act
            var result = await _controller.GetVenue(Guid.NewGuid());

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetVenue_ReturnsOkResult_WithVenue()
        {
            // Arrange
            var venue = new Venue { Id = Guid.NewGuid(), Name = "Venue 1" };
            _mockVenueRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(venue);

            // Act
            var result = await _controller.GetVenue(venue.Id);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(venue));
        }
    }
}