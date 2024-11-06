namespace TicketingSystem.DAL.Models;

public class Offer
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; }
    public Price Price { get; set; }
}

public class Price
{
    public int Id { get; set; }
    public int OfferId { get; set; }
    public Offer Offer { get; set; }
    public string PriceLevel { get; set; } // e.g., "Adult", "Child", "VIP"
    public decimal Amount { get; set; }
}