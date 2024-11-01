namespace TicketingSystem.DAL.Models;

public class Payment
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public Cart Cart { get; set; }
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
}

public enum PaymentStatus
{
    Pending = 0,
    Completed = 1,
    Failed = 2
}