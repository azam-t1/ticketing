using Microsoft.EntityFrameworkCore;
using TicketingSystem.DAL.Context;
using TicketingSystem.DAL.Models;
using TicketingSystem.DAL.Repositories.Base;

namespace TicketingSystem.DAL.Repositories;

public interface ICustomerRepository : IBaseRepository<Customer>
{
    Task<Customer?> GetCustomerByEmailAsync(string email);
    Task<bool> CheckPasswordAsync(string customerId, string password);
}

public class CustomerRepository(TicketingDbContext context) : BaseRepository<Customer>(context), ICustomerRepository
{
    private readonly TicketingDbContext _context = context;
    
    public async Task<Customer?> GetCustomerByEmailAsync(string email)
    {
        return await DbSet.FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<bool> CheckPasswordAsync(string customerId, string password)
    {
        var customer = await DbSet.FindAsync(customerId);
        if (customer != null)
        {
            // Implement password verification logic here
            return customer.PasswordHash == HashPassword(password);
        }

        return false;
    }

    // simple password hashing method
    private string HashPassword(string password)
    {
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
    }
}