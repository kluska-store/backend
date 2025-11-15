using System.Text.RegularExpressions;
using KluskaStore.Domain.Interfaces;
using KluskaStore.Domain.Shared;

namespace KluskaStore.Domain.ValueObjects;

public partial class Cnpj : ValueObject<string> {
  private Cnpj(string value) : base(value) { }

  public static Result<Cnpj> Create(string value, bool isMock = false) {
    return Validate(value, isMock)
      ? Result<Cnpj>.Success(new Cnpj(value)) 
      : Result<Cnpj>.Failure("Invalid email");
  }

  [GeneratedRegex(@"^\d{14}$")]
  private static partial Regex CnpjRegex();

  private static bool Validate(string cnpj, bool skipCheckDigits = false) {
    if (!CnpjRegex().IsMatch(cnpj)) return false;
    if (skipCheckDigits) return true;
    if (cnpj.Distinct().Count() == 1) return false;

    int[] weights = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
    int[] weights2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

    var sum = 0;
    for (var i = 0; i < 12; i++) sum += (int)char.GetNumericValue(cnpj[i]) * weights[i];
    var rest = sum % 11;
    var verifierDigit1 = rest is 1 or 0 ? 0 : 11 - rest;

    if (verifierDigit1 != (int)char.GetNumericValue(cnpj[12])) return false;

    sum = 0;
    for (var i = 0; i < 13; i++) sum += (int)char.GetNumericValue(cnpj[i]) * weights2[i];
    rest = sum % 11;
    var verifierDigit2 = rest is 1 or 0 ? 0 : 11 - rest;

    var isValid = verifierDigit2 == (int)char.GetNumericValue(cnpj[13]);
    return isValid;
  }
}