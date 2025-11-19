using KluskaStore.Domain.Interfaces;
using KluskaStore.Domain.Repositories;
using KluskaStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KluskaStore.Infrastructure.Repositories;

public class EntityRepository<T, TId>(AppDbContext context) : IEntityRepository<T, TId> where T : class, IEntity
{
    protected readonly AppDbContext Context = context;

    public virtual async Task<T?> GetByIdAsync(TId id) => await Context.Set<T>().FindAsync(id);

    public virtual async Task<IEnumerable<T>> GetAllAsync() => await Context.Set<T>().ToListAsync();

    public virtual async Task AddAsync(T entity) => await Context.Set<T>().AddAsync(entity);

    public virtual void UpdateAsync(T entity) => Context.Set<T>().Update(entity);

    public virtual void DeleteAsync(T entity) => Context.Set<T>().Remove(entity);
}