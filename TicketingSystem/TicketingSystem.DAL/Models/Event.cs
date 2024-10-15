namespace TicketingSystem.DAL.Models;

public class Event
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int VenueId { get; set; }
    public Venue Venue { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public ICollection<Offer> Offers { get; set; }
}