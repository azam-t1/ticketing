using Microsoft.AspNetCore.Mvc;
using TicketingSystem.API.Models.Order;
using TicketingSystem.DAL.Repositories;

namespace TicketingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(ICartRepository cartRepository) : ControllerBase
{
    [HttpGet("carts/{cartId}")]
    public async Task<IActionResult> GetCart(Guid cartId)
    {
        var cart = await cartRepository.GetByIdAsync(cartId);
        if (cart == null)
        {
            return NotFound();
        }
        return Ok(cart);
    }
    
    [HttpGet("carts/{cartId}")]
    public async Task<IActionResult> GetCartItems(Guid cartId)
    {
        var cart = await cartRepository.GetByIdAsync(cartId);
        if (cart == null)
            return NotFound();
        
        return Ok(new
        {
            id = cart.Id,
            totalAmount = cart.CartItems.Sum(ci => ci.Price.Amount),
            items = cart.CartItems.Select(ci => new
            {
                id = ci.Id,
                eventId = ci.EventId,
                seatId = ci.SeatId,
                priceId = ci.PriceId
            })
        });
    }

    [HttpPost("carts/{cartId}")]
    public async Task<IActionResult> AddSeatToCart(Guid cartId, [FromBody] AddSeatToCartRequest request)
    {
        var cart = await cartRepository.AddSeatToCartAsync(cartId, request.EventId, request.SeatId, request.PriceId);
        return CreatedAtAction("GetCartItems", new { cartId = cart.Id }, cart);
    }

    [HttpDelete("carts/{cartId}/events/{eventId}/seats/{seatId}")]
    public async Task<IActionResult> DeleteSeatFromCart(Guid cartId, Guid eventId, int seatId)
    {
        var result = await cartRepository.DeleteSeatFromCartAsync(cartId, eventId, seatId);
        if (!result)
            return NotFound();
        
        return NoContent();
    }

    [HttpPut("carts/{cartId}/book")]
    public async Task<IActionResult> BookCart(Guid cartId)
    {
        var paymentId = await cartRepository.BookCartAsync(cartId);
        if (paymentId == null)
        {
            return NotFound();
        }
        
        return Ok(new { PaymentId = paymentId });
    }

}
