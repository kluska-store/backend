using KluskaStore.Domain.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KluskaStore.Infrastructure.Persistence.Configuration.Aggregates;

public sealed class UserSessionConfiguration :
    SessionBaseConfigurations<UserSession>,
    IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        ConfigureBase(builder);

        builder.ToTable("user_session");
        builder.HasKey(s => s.Token);
        builder.Property(s => s.Token).ValueGeneratedNever();

        builder.HasOne<User>().WithMany().HasForeignKey(s => s.UserId);
        builder.Property(u => u.UserId).HasColumnName("user_id");
    }
}
