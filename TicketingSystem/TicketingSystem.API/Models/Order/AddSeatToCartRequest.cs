using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.API.Models.Order;

public class AddSeatToCartRequest
{
    [Required]
    public Guid EventId { get; set; }
    
    [Required]
    public int SeatId { get; set; }
    
    [Required]
    public int PriceId { get; set; }
}