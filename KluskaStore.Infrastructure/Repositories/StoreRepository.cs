using KluskaStore.Domain.Entities.Users;
using KluskaStore.Domain.Repositories;
using KluskaStore.Domain.ValueObjects.PersonalData;
using KluskaStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KluskaStore.Infrastructure.Repositories;

public class StoreRepository(AppDbContext context) : EntityRepository<Store, Guid>(context), IStoreRepository
{
    public override async Task<Store?> GetByIdAsync(Guid id) =>
        await Context.Stores
            .Include(s => s.Address)
            .FirstOrDefaultAsync(s => s.Id == id);

    public async Task<Store?> GetByCnpjAsync(Cnpj cnpj) =>
        await Context.Stores
            .Include(s => s.Address)
            .FirstOrDefaultAsync(s => s.Cnpj == cnpj);
}
