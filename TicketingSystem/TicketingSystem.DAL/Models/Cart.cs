﻿namespace TicketingSystem.DAL.Models;

public class Cart
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public ICollection<CartItem> CartItems { get; set; }
}

public class CartItem
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public Cart Cart { get; set; }
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; }
}