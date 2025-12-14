namespace KluskaStore.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    ISessionRepository Sessions { get; }

    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
