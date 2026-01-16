using KluskaStore.Domain.ValueObjects.AccountData;

namespace KluskaStore.Tests.Common.Builders;

public static class PhoneBuilder
{
    public static Phone Valid() => new("+55 (11) 99999-9999");
    public static Phone Invalid() => new("999999999");
}
