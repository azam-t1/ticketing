﻿using Microsoft.EntityFrameworkCore;
using TicketingSystem.DAL.Context;

namespace TicketingSystem.DAL.Repositories.Base;

public class BaseRepository<T>(TicketingDbContext context) : IBaseRepository<T>
    where T : class
{
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<IEnumerable<T?>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(T? entity)
    {
        await _dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T? entity)
    {
        _dbSet.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}