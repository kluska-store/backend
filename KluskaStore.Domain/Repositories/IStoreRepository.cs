using KluskaStore.Domain.Entities;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Domain.Repositories;

public interface IStoreRepository : IEntityRepository<Store, Guid>
{
    Task<Store?> GetByCnpjAsync(Cnpj cnpj);
}