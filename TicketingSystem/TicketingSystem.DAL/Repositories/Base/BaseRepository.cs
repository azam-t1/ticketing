using Microsoft.EntityFrameworkCore;
using TicketingSystem.DAL.Context;
using TicketingSystem.DAL.Models;

namespace TicketingSystem.DAL.Repositories.Base;

public class BaseRepository<T>(TicketingDbContext context) : IBaseRepository<T>
    where T : class
{
    protected readonly DbSet<T> DbSet = context.Set<T>();

    public async Task<IEnumerable<T?>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task AddAsync(T? entity)
    {
        await DbSet.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T? entity)
    {
        DbSet.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            DbSet.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}