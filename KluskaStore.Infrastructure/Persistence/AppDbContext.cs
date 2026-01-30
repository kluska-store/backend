using KluskaStore.Domain.Entities.Accounts;
using KluskaStore.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace KluskaStore.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Cart> Carts => Set<Cart>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Temporary until User and Address repository implementation and configuration
        modelBuilder.Entity<User>()
            .Ignore(u => u.Addresses)
            .Ignore(u => u.Cpf)
            .Ignore(u => u.Email)
            .Ignore(u => u.Phone);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
