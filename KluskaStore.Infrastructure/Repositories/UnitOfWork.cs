using KluskaStore.Domain.Repositories;
using KluskaStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace KluskaStore.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork {
  private readonly AppDbContext _context;
  private IDbContextTransaction? _transaction;
  private bool _disposed;

  public IStoreRepository Stores { get; }
  
  public UnitOfWork(AppDbContext context) {
    _context = context;
    Stores = new StoreRepository(_context);
  }

  public async Task BeginTransactionAsync() => _transaction ??= await _context.Database.BeginTransactionAsync();

  public async Task CommitTransactionAsync() {
    if (_transaction is not null) {
      await _transaction.CommitAsync();
      await _transaction.DisposeAsync();
      _transaction = null;
    }
  }

  public async Task RollbackTransactionAsync() {
    if (_transaction is not null) {
      await _transaction.RollbackAsync();
      await _transaction.DisposeAsync();
      _transaction = null;
    }
  }

  public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

  public void Dispose() {
    Dispose(disposing: true);
    GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing) {
    if (_disposed) return;
    if (disposing) {
      _transaction?.Dispose();
      _context.Dispose();
    }

    _disposed = true;
  }
}