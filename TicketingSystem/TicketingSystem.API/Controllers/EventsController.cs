using Microsoft.AspNetCore.Mvc;
using TicketingSystem.DAL.Repositories;

namespace TicketingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController(IEventRepository eventRepository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetEvents()
    {
        var events = await eventRepository.GetAllAsync();
        return Ok(events);
    }

    [HttpGet("{eventId}/sections/{sectionId}/seats")]
    public async Task<IActionResult> GetSeats(Guid eventId, int sectionId)
    {
        var eventEntity = await eventRepository.GetByIdAsync(eventId);
        if (eventEntity == null)
        {
            return NotFound();
        }
        
        var section = eventEntity.Venue.Sections.FirstOrDefault(s => s.Id == sectionId);
        if (section == null)
        {
            return NotFound();
        }

        var seats = section.Seats.Select(seat => new
        {
            sectionId = seat.Row.SectionId,
            rowId = seat.RowId,
            seatId = seat.Id,
            seatStatus = new
            {
                id = seat.Status,
                name = seat.Status.ToString(),
            },
            price = new
            {
                id = seat.Price.Id,
                name = seat.Price.PriceLevel,
                amount = seat.Price.Amount,
            }
        });

        return Ok(seats);
    }
}