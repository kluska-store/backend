using KluskaStore.Domain.Entities.Accounts;
using KluskaStore.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KluskaStore.Infrastructure.Persistence.Configuration.Aggregates;

public sealed class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("carts");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.HasIndex(c => c.UserId);

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<Cart>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsMany(c => c.Items, item =>
        {
            item.ToTable("cart_items");

            item.HasKey(i => i.Id);
            item.Property(i => i.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            const string fkShadowProperty = "ProductId";
            item.Property<Guid>(fkShadowProperty)
                .HasColumnName("product_id")
                .IsRequired();

            item.HasOne<Product>()
                .WithMany()
                .HasForeignKey(fkShadowProperty)
                .OnDelete(DeleteBehavior.Cascade);

            item.Property(i => i.Quantity).IsRequired();
        });
    }
}
