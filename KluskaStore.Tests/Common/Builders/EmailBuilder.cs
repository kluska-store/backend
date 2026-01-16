using KluskaStore.Domain.ValueObjects.AccountData;

namespace KluskaStore.Tests.Common.Builders;

public static class EmailBuilder
{
    public static Email Valid() => new("example@email.com");
    public static Email Invalid() => new("exampleemail.com");
}
