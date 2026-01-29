using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace KluskaStore.Infrastructure.Persistence.Repositories;

public sealed class ProductRepository(AppDbContext context) : IProductRepository
{
    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
}
