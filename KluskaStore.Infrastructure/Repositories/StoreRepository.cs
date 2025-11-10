using KluskaStore.Domain.Entities;
using KluskaStore.Domain.Repositories;
using KluskaStore.Domain.ValueObjects;
using KluskaStore.Infrastructure.Data;

namespace KluskaStore.Infrastructure.Repositories;

public class StoreRepository(AppDbContext context) : EntityRepository<Store, Guid>(context), IStoreRepository {
  public async Task<Store?> GetByCnpjAsync(Cnpj cnpj) => await Context.Stores.FindAsync(cnpj);
}