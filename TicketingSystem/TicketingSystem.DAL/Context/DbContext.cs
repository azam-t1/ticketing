using TicketingSystem.DAL.Models;

namespace TicketingSystem.DAL.Context;

using Microsoft.EntityFrameworkCore;

public class TicketingDbContext(DbContextOptions<TicketingDbContext> options) : DbContext(options)
{
    public DbSet<Event> Events { get; set; }
    public DbSet<Venue> Venues { get; set; }
    public DbSet<Manifest> Manifests { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<Price> Prices { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<EventManager> EventManagers { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define relationships, constraints, etc.
        modelBuilder.Entity<Event>()
            .HasOne(e => e.Venue)
            .WithMany()
            .HasForeignKey(e => e.VenueId);

        modelBuilder.Entity<Offer>()
            .HasOne(o => o.Event)
            .WithMany(e => e.Offers)
            .HasForeignKey(o => o.EventId);
        
        modelBuilder.Entity<Event>()
            .HasOne(e => e.Venue)
            .WithMany()
            .HasForeignKey(e => e.VenueId);

        modelBuilder.Entity<Venue>()
            .HasOne(v => v.Manifest)
            .WithMany()
            .HasForeignKey(v => v.ManifestId);

        modelBuilder.Entity<Manifest>()
            .HasMany(m => m.Sections)
            .WithOne(s => s.Manifest)
            .HasForeignKey(s => s.ManifestId);

        modelBuilder.Entity<Section>()
            .HasMany(s => s.Rows)
            .WithOne(r => r.Section)
            .HasForeignKey(r => r.SectionId);

        modelBuilder.Entity<Row>()
            .HasMany(r => r.Seats)
            .WithOne(s => s.Row)
            .HasForeignKey(s => s.RowId);

        modelBuilder.Entity<Offer>()
            .HasOne(o => o.Event)
            .WithMany(e => e.Offers)
            .HasForeignKey(o => o.EventId);

        modelBuilder.Entity<Price>()
            .HasOne(p => p.Offer)
            .WithMany(o => o.Prices)
            .HasForeignKey(p => p.OfferId);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Event)
            .WithMany()
            .HasForeignKey(t => t.EventId);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Seat)
            .WithMany()
            .HasForeignKey(t => t.SeatId);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Customer)
            .WithMany(c => c.Tickets)
            .HasForeignKey(t => t.CustomerId);

        // Additional configurations can be added here
    }
}