using System.Text.RegularExpressions;
using KluskaStore.Domain.Interfaces;
using KluskaStore.Domain.Shared;

namespace KluskaStore.Domain.ValueObjects;

public partial class Phone : ValueObject<string> {
  private Phone(string value) : base(value) { }

  public static Result<Phone> Create(string value) {
    return PhoneRegex().IsMatch(value)
      ? Result<Phone>.Success(new Phone(value))
      : Result<Phone>.Failure("Invalid phone");
  }

  [GeneratedRegex(@"^\+?\d{12,15}$")]
  private static partial Regex PhoneRegex();
}