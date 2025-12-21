using KluskaStore.Domain.Entities.Products;

namespace KluskaStore.Application.Abstractions.Persistence;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
