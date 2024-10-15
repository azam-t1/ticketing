using Microsoft.EntityFrameworkCore;
using TicketingSystem.DAL.Context;
using TicketingSystem.DAL.Models;
using TicketingSystem.DAL.Repositories.Base;

namespace TicketingSystem.DAL.Repositories;

public interface IPaymentRepository : IBaseRepository<Payment>
{
    Task<IEnumerable<Payment>> GetPaymentsByCustomerIdAsync(int customerId);
    Task<Payment> CreatePaymentAsync(int ticketId, decimal amount, string paymentMethod);
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

    public async Task<Payment> CreatePaymentAsync(int ticketId, decimal amount, string paymentMethod)
    {
        var payment = new Payment
        {
            TicketId = ticketId,
            Amount = amount,
            PaymentDate = DateTime.UtcNow,
            PaymentMethod = paymentMethod,
            PaymentStatus = "Pending"
        };

        await _dbSet.AddAsync(payment);
        await _context.SaveChangesAsync();

        return payment;
    }
}