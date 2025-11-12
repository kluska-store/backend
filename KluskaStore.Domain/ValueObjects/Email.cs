using System.Text.RegularExpressions;
using KluskaStore.Domain.Interfaces;

namespace KluskaStore.Domain.ValueObjects;

public partial class Email : ValueObject<string>, ICreatableVo<Email> {
  private Email(string value) : base(value) { }

  public static VoValidationResult<Email> New(string value) {
    if (EmailRegex().IsMatch(value))
      return new VoValidationResult<Email>(true, new Email(value), null);
    return new VoValidationResult<Email>(false, null, $"Email {value} is invalid");
  }

  [GeneratedRegex(@"^\w+(\.\w+)*@\w+(\.\w+)+$", RegexOptions.Compiled)]
  private static partial Regex EmailRegex();
}