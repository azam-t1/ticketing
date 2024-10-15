namespace TicketingSystem.DAL.Models;

public class Customer
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string? PasswordHash { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
}