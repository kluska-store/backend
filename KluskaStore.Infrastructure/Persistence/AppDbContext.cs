using KluskaStore.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace KluskaStore.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Cart> Carts => Set<Cart>();
}
