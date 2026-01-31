using KluskaStore.Domain.Entities.Accounts;

namespace KluskaStore.Tests.Common.Builders;

public static class UserSessionBuilder
{
    private static UserSession Generate(
        string? token = null,
        Guid? userId = null,
        DateTime? creationDate = null,
        DateTime? expirationDate = null
    ) => new(
        token ?? "session token",
        userId ?? Guid.NewGuid(),
        creationDate ?? DateTime.UtcNow
    )
    { ExpiresAt = expirationDate ?? default };

    public static UserSession Valid() => Generate();

    public static UserSession WithUserId(Guid userId) => Generate(userId: userId);

    public static UserSession EmptyToken() => Generate(token: "");

    public static UserSession Expired() => Generate(
        creationDate: DateTime.UtcNow.AddYears(-2),
        expirationDate: DateTime.UtcNow.AddYears(-1)
    );

    public static UserSession InvalidCreationDate() => Generate(creationDate: DateTime.UtcNow.AddDays(1));
}
