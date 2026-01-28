namespace KluskaStore.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    ISessionRepository Sessions { get; }
    IProductRepository Products { get; }
    ICartRepository Carts { get; }
    IItemRepository Items { get; }

    Task CommitAsync(CancellationToken cancellationToken = default);
}
