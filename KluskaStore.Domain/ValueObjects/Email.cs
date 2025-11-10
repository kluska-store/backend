using System.Text.RegularExpressions;

namespace KluskaStore.Domain.ValueObjects;

public partial class Email : ValueObject<string> {
  public Email(string value) : base(value) {
    if (!EmailRegex().IsMatch(value)) throw new ArgumentException("Invalid email.", nameof(value));
  }

  [GeneratedRegex(@"\w+(\.\w+)*@\w+(\.\w+)+", RegexOptions.Compiled)]
  private static partial Regex EmailRegex();
}