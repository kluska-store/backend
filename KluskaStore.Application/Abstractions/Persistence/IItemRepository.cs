using KluskaStore.Domain.Entities.Products;

namespace KluskaStore.Application.Abstractions.Persistence;

// TODO: make Item an owned type, therefore move its methods to their owner's repository
public interface IItemRepository
{
    Task<Item?> GetByIdAsync(Guid itemId, CancellationToken cancellationToken = default);
}
