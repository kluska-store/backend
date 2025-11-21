using KluskaStore.Domain.Interfaces;

namespace KluskaStore.Domain.Repositories;

public interface IEntityRepository<T, in TId> where T : class, IEntity
{
    Task<T?> GetByIdAsync(TId id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    void UpdateAsync(T entity);
    Task DeleteAsync(TId id);
}
