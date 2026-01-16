using KluskaStore.Domain.ValueObjects.AccountData;

namespace KluskaStore.Tests.Common.Builders;

public static class CpfBuilder
{
    public static Cpf Valid() => new("01234567890");
    public static Cpf Invalid() => new("00000000000");
}
