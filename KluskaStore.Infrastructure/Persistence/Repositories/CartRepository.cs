using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace KluskaStore.Infrastructure.Persistence.Repositories;

public sealed class CartRepository(AppDbContext context) : ICartRepository
{
    public async Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        => await context.Carts.FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

    public async Task AddAsync(Cart cart, CancellationToken cancellationToken = default)
        => await context.Carts.AddAsync(cart, cancellationToken);
}
