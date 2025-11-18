using KluskaStore.Domain.Repositories;
using KluskaStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace KluskaStore.Infrastructure.Repositories;

public sealed class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private IDbContextTransaction? _transaction;

    public IStoreRepository Stores { get; } = new StoreRepository(context);
    public IAddressRepository Addresses { get; } = new AddressRepository(context);

    public async Task BeginTransactionAsync() => _transaction ??= await context.Database.BeginTransactionAsync();

    public async Task CommitTransactionAsync()
    {
        if (_transaction is not null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task<int> SaveChangesAsync() => await context.SaveChangesAsync();

    public void Dispose()
    {
        _transaction?.Dispose();
        context.Dispose();
    }
}