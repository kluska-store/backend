namespace KluskaStore.Domain.Errors.ValueObjects;

using static Error;

public static class CnpjErrors
{
    public static readonly Error InvalidCnpj = Validation("Cnpj.InvalidCnpj", "Invalid CNPJ");
}
