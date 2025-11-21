using System.Text.RegularExpressions;
using KluskaStore.Domain.Interfaces;
using KluskaStore.Domain.Shared;

namespace KluskaStore.Domain.ValueObjects;

public partial class Cpf : IValueObject
{
    private Cpf(string value) => Value = value;

    public string Value { get; }

    [GeneratedRegex(@"\d{11}")]
    private static partial Regex CpfRegex();

    private static bool Validate(string cpf, bool skipCheckDigits)
    {
        if (!CpfRegex().IsMatch(cpf)) return false;
        if (skipCheckDigits) return true;
        if (cpf.Distinct().Count() == 1) return false;

        var sum = 0;
        List<int> weights = [10, 9, 8, 7, 6, 5, 4, 3, 2];

        for (var i = 0; i < 9; i++) sum += weights[i] * (int)char.GetNumericValue(cpf[i]);
        sum %= 11;
        var verifierDigit = sum is 1 or 0 ? 0 : 11 - sum;

        if (verifierDigit != (int)char.GetNumericValue(cpf[9])) return false;

        sum = 0;
        weights.Insert(0, 11);

        for (var i = 0; i < 10; i++) sum += weights[i] * (int)char.GetNumericValue(cpf[i]);
        sum %= 11;
        verifierDigit = sum is 1 or 0 ? 0 : 11 - sum;

        return verifierDigit == (int)char.GetNumericValue(cpf[10]);
    }

    public static Result<Cpf> Create(string value, bool skipVerifierDigitsValidation = false) =>
        Validate(value, skipVerifierDigitsValidation)
            ? Result<Cpf>.Success(new Cpf(value))
            : Result<Cpf>.Failure("Invalid cpf");
}
