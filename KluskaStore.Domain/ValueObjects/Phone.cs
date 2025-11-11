using System.Text.RegularExpressions;

namespace KluskaStore.Domain.ValueObjects;

public partial class Phone : ValueObject<string> {
  public Phone() { }

  public Phone(string value) : base(value) {
    if (!PhoneRegex().IsMatch(value)) throw new ArgumentException("Invalid Phone.", nameof(value));
  }

  [GeneratedRegex(@"\+?\d{12,15}")]
  private static partial Regex PhoneRegex();
}