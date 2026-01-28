using KluskaStore.Domain.Entities.Products;

namespace KluskaStore.Application.Abstractions.Persistence;

public interface IItemRepository
{
    Task<Item?> GetByIdAsync(Guid itemId, CancellationToken cancellationToken = default);
}
