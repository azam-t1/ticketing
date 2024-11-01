using Microsoft.EntityFrameworkCore;
using TicketingSystem.DAL.Context;
using TicketingSystem.DAL.Models;
using TicketingSystem.DAL.Repositories.Base;

namespace TicketingSystem.DAL.Repositories;

public interface IPaymentRepository : IBaseRepository<Payment>
{
    Task<IEnumerable<Payment>> GetPaymentsByCustomerIdAsync(int customerId);
    Task<Payment> CreatePaymentAsync(Guid cartId, decimal amount);
    Task CompletePaymentAsync(Guid paymentId);
    Task FailPaymentAsync(Guid paymentId);
}

public class PaymentRepository(TicketingDbContext context) : BaseRepository<Payment>(context), IPaymentRepository
{
    private readonly TicketingDbContext _context = context;
    
    public async Task<IEnumerable<Payment>> GetPaymentsByCustomerIdAsync(int customerId)
    {
        var tickets = await _context.Tickets
            .Where(t => t.CustomerId == customerId)
            .Select(t => t.Id)
            .ToListAsync();

        return await _dbSet
            .Where(p => tickets.Contains(p.TicketId))
            .ToListAsync();
    }

    public async Task<Payment> CreatePaymentAsync(Guid cartId, decimal amount)
    {
        var payment = new Payment
        {
            CartId = cartId,
            // TicketId = ticketId,
            Amount = amount,
            PaymentDate = DateTime.UtcNow,
            // PaymentMethod = paymentMethod,
            PaymentStatus = PaymentStatus.Pending
        };

        await _dbSet.AddAsync(payment);
        await _context.SaveChangesAsync();

        return payment;
    }
    
    public async Task CompletePaymentAsync(Guid paymentId)
    {
        var payment = await _dbSet.FindAsync(paymentId);
        if (payment == null)
            throw new KeyNotFoundException("Payment not found");

        payment.PaymentStatus = PaymentStatus.Completed;
        var paymentSeats = payment.Cart.CartItems.Select(x => x.Seat).ToList();
        foreach (var paymentSeat in paymentSeats)
        {
            paymentSeat.Status = SeatStatus.Sold;
        }
        
        await _context.SaveChangesAsync();
    }
    
    public async Task FailPaymentAsync(Guid paymentId)
    {
        var payment = await _dbSet.FindAsync(paymentId);
        if (payment == null)
            throw new KeyNotFoundException("Payment not found");

        payment.PaymentStatus = PaymentStatus.Completed;
        var paymentSeats = payment.Cart.CartItems.Select(x => x.Seat).ToList();
        foreach (var paymentSeat in paymentSeats)
        {
            paymentSeat.Status = SeatStatus.Available;
        }
        
        await _context.SaveChangesAsync();
    }
}