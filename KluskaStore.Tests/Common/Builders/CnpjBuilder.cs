using KluskaStore.Domain.ValueObjects.AccountData;

namespace KluskaStore.Tests.Common.Builders;

public static class CnpjBuilder
{
    public static Cnpj Valid() => new("04658924000130");
    public static Cnpj Invalid() => new("00000000000000");
}
