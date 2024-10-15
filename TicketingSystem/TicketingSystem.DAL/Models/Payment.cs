namespace TicketingSystem.DAL.Models;

public class Payment
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentStatus { get; set; }
}