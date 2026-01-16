using KluskaStore.Domain.Entities.Accounts;

namespace KluskaStore.Tests.Common.Builders;

public static class StoreBuilder
{
    private static Store Generate(string? name = null, string? passwordHash = null) => new(
        CnpjBuilder.Valid(),
        name ?? "store",
        EmailBuilder.Valid(),
        passwordHash ?? "password",
        AddressBuilder.Valid()
    );

    public static Store Valid() => Generate();
    public static Store NoName() => Generate(name: "");
    public static Store EmptyPassword() => Generate(passwordHash: "");
}
