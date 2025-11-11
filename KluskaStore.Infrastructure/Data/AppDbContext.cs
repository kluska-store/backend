using Microsoft.EntityFrameworkCore;
using KluskaStore.Domain.Entities;
using KluskaStore.Infrastructure.Configurations;

namespace KluskaStore.Infrastructure.Data;

public class AppDbContext : DbContext {
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
  
  public DbSet<Product> Products { get; set; } // sample
  public DbSet<Store> Stores { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    base.OnModelCreating(modelBuilder);

    modelBuilder.ApplyConfiguration(new StoreConfiguration());
  }
}