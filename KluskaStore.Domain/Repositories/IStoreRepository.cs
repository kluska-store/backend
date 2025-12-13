using KluskaStore.Domain.Entities.Users;

namespace KluskaStore.Domain.Repositories;

public interface IStoreRepository
{
    Task<Store?> GetByIdAsync(Guid id);
    Task AddAsync(Store store);
}
