namespace KluskaStore.Domain.Errors.ValueObjects;

using static Error;

public static class CpfErrors
{
    public static readonly Error InvalidCpf = Validation("Cpf.InvalidCpf", "Invalid CPF");
}
