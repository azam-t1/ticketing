using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.API.Models.Order;

public class AddSeatToCartRequest
{
    [Required]
    public int EventId { get; set; }
    
    [Required]
    public int SeatId { get; set; }
    
    [Required]
    public int PriceId { get; set; }
}