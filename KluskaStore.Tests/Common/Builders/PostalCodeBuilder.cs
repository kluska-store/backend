using KluskaStore.Domain.ValueObjects.AccountData.Address;

namespace KluskaStore.Tests.Common.Builders;

public static class PostalCodeBuilder
{
    public static PostalCode Valid() => new("00000000");
    public static PostalCode Invalid() => new("");
}
