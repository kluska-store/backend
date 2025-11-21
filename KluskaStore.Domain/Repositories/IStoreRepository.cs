using KluskaStore.Domain.Entities;

namespace KluskaStore.Domain.Repositories;

public interface IStoreRepository
{
    Task<Store?> GetByIdAsync(Guid id);
    Task AddAsync(Store store);
}
