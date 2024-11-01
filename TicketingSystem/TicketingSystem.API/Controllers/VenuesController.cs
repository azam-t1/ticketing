using Microsoft.AspNetCore.Mvc;
using TicketingSystem.DAL.Repositories;

namespace TicketingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VenuesController(IVenueRepository venueRepository) : ControllerBase
{
    [HttpGet("{venueId}")]
    public async Task<IActionResult> GetVenue(Guid newGuid)
    {
        var venue = await venueRepository.GetByIdAsync(newGuid);
        if (venue == null)
        {
            return NotFound();
        }
        return Ok(venue);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetVenues()
    {
        var venues = await venueRepository.GetAllAsync();
        return Ok(venues);
    }

    [HttpGet("{venueId}/sections")]
    public async Task<IActionResult> GetSections(Guid venueId)
    {
        var venue = await venueRepository.GetByIdAsync(venueId);
        if (venue == null)
        {
            return NotFound();
        }
        return Ok(venue.Sections);
    }

}