namespace KluskaStore.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    IStoreRepository Stores { get; }
    IAddressRepository Addresses { get; }
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task<int> SaveChangesAsync();
}
