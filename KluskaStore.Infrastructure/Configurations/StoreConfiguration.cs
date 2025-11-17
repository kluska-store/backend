using KluskaStore.Domain.Entities;
using KluskaStore.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KluskaStore.Infrastructure.Configurations;

public class StoreConfiguration : IEntityTypeConfiguration<Store> {
  public void Configure(EntityTypeBuilder<Store> builder) {
    builder.HasKey(s => s.Id);

    builder.Property(s => s.Cnpj)
      .HasConversion(c => c.Value, v => Cnpj.Create(v, false).Value)
      .HasMaxLength(14)
      .IsRequired();

    builder.Property(s => s.Email)
      .HasConversion(e => e.Value, v => Email.Create(v).Value)
      .HasMaxLength(100)
      .IsRequired();

    builder.Property(s => s.Name)
      .HasMaxLength(200)
      .IsRequired();

    builder.Property(s => s.PasswordHash)
      .HasMaxLength(200)
      .IsRequired();

    builder.Property(s => s.IsActive).IsRequired();
  }
}