namespace TicketingSystem.DAL.Models;

public class Notification
{
    public int Id { get; set; }
    public int RecipientId { get; set; } // CustomerId or EventManagerId
    public string Message { get; set; }
    public DateTime NotificationDate { get; set; }
    public string NotificationType { get; set; } // e.g., "Purchase", "Cancellation", etc.
}