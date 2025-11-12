using System.Text.RegularExpressions;
using KluskaStore.Domain.Interfaces;

namespace KluskaStore.Domain.ValueObjects;

public partial class Phone : ValueObject<string>, ICreatableVo<Phone> {
  private Phone(string value) : base(value) { }

  public static VoValidationResult<Phone> New(string value) {
    if (PhoneRegex().IsMatch(value))
      return new VoValidationResult<Phone>(true, new Phone(value), null);
    return new VoValidationResult<Phone>(false, null, $"Phone {value} is invalid");
  }

  [GeneratedRegex(@"^\+?\d{12,15}$")]
  private static partial Regex PhoneRegex();
}