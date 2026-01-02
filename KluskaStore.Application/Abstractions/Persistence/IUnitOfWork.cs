namespace KluskaStore.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    ISessionRepository Sessions { get; }
    IProductRepository Products { get; }

    Task CommitAsync(CancellationToken cancellationToken = default);
}
