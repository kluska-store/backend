using KluskaStore.Domain.Entities;
using KluskaStore.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KluskaStore.Infrastructure.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Country)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(a => a.State)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(a => a.City)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.Street)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.Complement)
            .HasMaxLength(100);

        builder.Property(a => a.PostalCode)
            .HasConversion(p => p.Value, v => PostalCode.Create(v).Value)
            .HasMaxLength(8)
            .IsRequired();
    }
}