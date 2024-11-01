using TicketingSystem.DAL.Models;
using TicketingSystem.DAL.Repositories;

namespace TicketingSystem.Tests.DAL.Repositories;

public class FakePaymentRepository : IPaymentRepository
{
    private readonly List<Payment> _payments = [];
    
    public Task<IEnumerable<Payment?>> GetAllAsync()
    {
        return Task.FromResult(_payments.AsEnumerable())!;
    }

    public Task<Payment?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_payments.FirstOrDefault(p => p.Id == id));
    }

    public Task AddAsync(Payment entity)
    {
        _payments.Add(entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Payment entity)
    {
        var existingPayment = _payments.FirstOrDefault(p => p.Id == entity.Id);
        if (existingPayment != null)
        {
            existingPayment.CartId = entity.CartId;
            existingPayment.Amount = entity.Amount;
            existingPayment.PaymentStatus = entity.PaymentStatus;
            existingPayment.PaymentDate = entity.PaymentDate;
        }
        
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        var paymentToRemove = _payments.FirstOrDefault(p => p.Id == id);
        if (paymentToRemove != null)
        {
            _payments.Remove(paymentToRemove);
        }
        
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Payment>> GetPaymentsByCustomerIdAsync(int customerId)
    {
        return Task.FromResult(_payments.Where(p => p.Cart.CustomerId == customerId).AsEnumerable());
    }

    public Task<Payment> CreatePaymentAsync(Guid cartId, decimal amount)
    {
        var payment = new Payment
        {
            CartId = cartId,
            Amount = amount,
            PaymentDate = DateTime.UtcNow,
            PaymentStatus = PaymentStatus.Pending
        };

        _payments.Add(payment);
        
        return Task.FromResult(payment);
    }

    public Task CompletePaymentAsync(Guid paymentId)
    {
        var payment = _payments.FirstOrDefault(p => p.Id == paymentId);
        if (payment == null)
            throw new KeyNotFoundException("Payment not found");
        
        payment.PaymentStatus = PaymentStatus.Completed;
        var paymentSeats = payment.Cart.CartItems.Select(x => x.Seat).ToList();
        foreach (var paymentSeat in paymentSeats)
        {
            paymentSeat.Status = SeatStatus.Sold;
        }
        
        return Task.CompletedTask;
    }

    public Task FailPaymentAsync(Guid paymentId)
    {
        var payment = _payments.FirstOrDefault(p => p.Id == paymentId);
        if (payment == null)
            throw new KeyNotFoundException("Payment not found");
        
        payment.PaymentStatus = PaymentStatus.Failed;
        var paymentSeats = payment.Cart.CartItems.Select(x => x.Seat).ToList();
        foreach (var paymentSeat in paymentSeats)
        {
            paymentSeat.Status = SeatStatus.Available;
        }
        
        return Task.CompletedTask;
    }
}