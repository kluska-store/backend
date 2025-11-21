using KluskaStore.Domain.Entities;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Domain.Repositories;

public interface IStoreRepository
{
    Task<Store?> GetByIdAsync(Guid id);
    Task AddAsync(Store store);
}
