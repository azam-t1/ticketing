using TicketingSystem.DAL.Context;
using TicketingSystem.DAL.Models;
using TicketingSystem.DAL.Repositories.Base;

namespace TicketingSystem.DAL.Repositories;

public interface INotificationRepository : IBaseRepository<Notification>
{
    Task SendPaymentConfirmationNotificationAsync(int customerId, int paymentId);
    Task SendPaymentRejectionNotificationAsync(int customerId, int paymentId, string reason);
}

public class NotificationRepository(TicketingDbContext context) : BaseRepository<Notification>(context), INotificationRepository
{
    private readonly TicketingDbContext _context = context;
    
    public async Task SendPaymentConfirmationNotificationAsync(int customerId, int paymentId)
    {
        var payment = await _context.Payments.FindAsync(paymentId);
        if (payment != null)
        {
            var notification = new Notification
            {
                RecipientId = customerId,
                Message = $"Your payment for ticket #{payment.TicketId} has been confirmed.",
                NotificationDate = DateTime.UtcNow,
                NotificationType = "PaymentConfirmation"
            };

            await _dbSet.AddAsync(notification);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SendPaymentRejectionNotificationAsync(int customerId, int paymentId, string reason)
    {
        var payment = await _context.Payments.FindAsync(paymentId);
        if (payment != null)
        {
            var notification = new Notification
            {
                RecipientId = customerId,
                Message = $"Your payment for ticket #{payment.TicketId} has been rejected. Reason: {reason}",
                NotificationDate = DateTime.UtcNow,
                NotificationType = "PaymentRejection"
            };

            await _dbSet.AddAsync(notification);
            await _context.SaveChangesAsync();
        }
    }
}