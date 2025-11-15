using System.Text.RegularExpressions;
using KluskaStore.Domain.Interfaces;
using KluskaStore.Domain.Shared;

namespace KluskaStore.Domain.ValueObjects;

public partial class Email : ValueObject<string> {
  private Email(string value) : base(value) { }

  public static Result<Email> Create(string value) {
    return EmailRegex().IsMatch(value)
      ? Result<Email>.Success(new Email(value))
      : Result<Email>.Failure("Invalid email");
  }

  [GeneratedRegex(@"^\w+(\.\w+)*@\w+(\.\w+)+$", RegexOptions.Compiled)]
  private static partial Regex EmailRegex();
}