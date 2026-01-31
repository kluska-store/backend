using KluskaStore.Domain.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KluskaStore.Infrastructure.Persistence.Configuration.Aggregates;

public abstract class SessionBaseConfigurations<TSession> where TSession : Session
{
    private static DateTime? NormalizeExpirationDate(DateTime date) => date == default ? null : date;

    public static void ConfigureBase(EntityTypeBuilder<TSession> builder)
    {
        builder.Property(s => s.Token)
            .HasColumnName("token")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.CreatedAt)
            .HasColumnName("creation_date")
            .IsRequired();

        builder.Property(s => s.ExpiresAt)
            .HasColumnName("expiration_date")
            .HasConversion(
                d => NormalizeExpirationDate(d),
                d => d ?? default
            );
    }
}
