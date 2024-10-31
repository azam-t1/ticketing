using Microsoft.AspNetCore.Mvc;
using TicketingSystem.DAL.Repositories;

namespace TicketingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController(IPaymentRepository paymentRepository) : ControllerBase
{
    [HttpGet("{paymentId}")]
    public async Task<IActionResult> GetPaymentStatus(Guid paymentId)
    {
        var payment = await paymentRepository.GetByIdAsync(paymentId);
        if (payment == null)
        {
            return NotFound();
        }

        return Ok(new
        {
            id = payment.Id, 
            status = payment.PaymentStatus
        });
    }
    
    [HttpPost("{paymentId}/complete")]
    public async Task<IActionResult> CompletePayment(Guid paymentId)
    {
        try
        {
            await paymentRepository.CompletePaymentAsync(paymentId);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
    
    [HttpPost("{paymentId}/failed")]
    public async Task<IActionResult> FailPayment(Guid paymentId)
    {
        try
        {
            await paymentRepository.FailPaymentAsync(paymentId);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}