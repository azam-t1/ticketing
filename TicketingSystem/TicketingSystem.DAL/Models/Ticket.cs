namespace TicketingSystem.DAL.Models;

public class Ticket
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; }
    public int SeatId { get; set; }
    public Seat Seat { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
}