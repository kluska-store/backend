using KluskaStore.Domain.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KluskaStore.Infrastructure.Persistence.Configuration.Aggregates;

public sealed class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.OwnsOne(u => u.Cpf,
            cpf => cpf.Property(c => c.Value)
                .HasColumnName("cpf")
                .IsRequired()
                .HasMaxLength(11)
        );

        builder.OwnsOne(u => u.Email,
            email => email.Property(e => e.Value)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(150)
        );

        builder.Property(u => u.Username)
            .HasColumnName("username")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.ProfilePicture)
            .HasColumnName("profile_pic_url")
            .HasMaxLength(255);

        builder.Property(u => u.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.OwnsOne(u => u.Phone,
            phone => phone.Property(p => p.FullPhone)
                .HasColumnName("phone_number")
                .IsRequired()
                .HasMaxLength(15)
        );

        builder.Property(u => u.Birthday)
            .HasColumnName("birthday")
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .HasColumnName("password_hash")
            .IsRequired()
            .HasMaxLength(255);

        builder.OwnsMany(u => u.Addresses, address =>
        {
            const string pkShadowProperty = "Id";
            address.ToTable("user_address");
            address.HasKey(pkShadowProperty);
            address.Property<Guid>(pkShadowProperty)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            address.WithOwner().HasForeignKey("user_id");

            address.Property(a => a.Country)
                .HasColumnName("country")
                .IsRequired()
                .HasMaxLength(50);

            address.Property(a => a.State)
                .HasColumnName("state")
                .IsRequired()
                .HasMaxLength(50);

            address.Property(a => a.City)
                .HasColumnName("city")
                .IsRequired()
                .HasMaxLength(50);

            address.Property(a => a.Street)
                .HasColumnName("street")
                .IsRequired()
                .HasMaxLength(50);

            address.Property(a => a.Number)
                .HasColumnName("number")
                .IsRequired();

            address.OwnsOne(a => a.PostalCode,
                postalCode => postalCode.Property(p => p.Value)
                    .HasColumnName("postal_code")
                    .IsRequired()
                    .HasMaxLength(8)
            );

            address.Property(a => a.Complement)
                .HasColumnName("complement")
                .HasMaxLength(50);
        });
    }
}
