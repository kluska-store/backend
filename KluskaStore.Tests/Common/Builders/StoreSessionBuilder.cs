using KluskaStore.Domain.Entities.Accounts;

namespace KluskaStore.Tests.Common.Builders;

public static class StoreSessionBuilder
{
    private static StoreSession Generate(
        string? token = null,
        Guid? storeId = null,
        DateTime? creationDate = null,
        DateTime? expirationDate = null
    ) => new(
        token ?? "session token",
        storeId ?? Guid.NewGuid(),
        creationDate ?? DateTime.UtcNow
    )
    { ExpiresAt = expirationDate ?? default };

    public static StoreSession Valid() => Generate();

    public static StoreSession WithStoreId(Guid storeId) => Generate(storeId: storeId);

    public static StoreSession EmptyToken() => Generate(token: "");

    public static StoreSession Expired() => Generate(
        creationDate: DateTime.UtcNow.AddYears(-2),
        expirationDate: DateTime.UtcNow.AddYears(-1)
    );

    public static StoreSession InvalidCreationDate() => Generate(creationDate: DateTime.UtcNow.AddDays(1));
}
