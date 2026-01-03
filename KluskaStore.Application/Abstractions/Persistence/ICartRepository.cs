using KluskaStore.Domain.Entities.Products;

namespace KluskaStore.Application.Abstractions.Persistence;

public interface ICartRepository
{
    Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(Cart cart, CancellationToken cancellationToken = default);
}
