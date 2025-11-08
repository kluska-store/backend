using Microsoft.EntityFrameworkCore;
using KluskaStore.Domain.Entities;

namespace KluskaStore.Infrastructure.Data;

public class AppDbContext : DbContext {
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    base.OnModelCreating(modelBuilder);
    
    // TODO: adicionar os mapeamentos com Fluent API
  }
}