using TicketingSystem.DAL.Models;

namespace TicketingSystem.DAL.Repositories.Base;

public interface IBaseRepository<T> where T : class
{
    Task<IEnumerable<T?>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}