using KluskaStore.Domain.Entities;
using KluskaStore.Domain.Repositories;
using KluskaStore.Infrastructure.Data;

namespace KluskaStore.Infrastructure.Repositories;

public class AddressRepository(AppDbContext context) : EntityRepository<Address, uint>(context), IAddressRepository;