using System.Text.RegularExpressions;
using KluskaStore.Domain.Errors.ValueObjects;
using KluskaStore.Domain.Interfaces;

namespace KluskaStore.Domain.ValueObjects.AccountData;

public partial class Email : IValueObject
{
    internal Email(string value) => Value = value;

    public string Value { get; }

    public static Result<Email> Create(string value) =>
        EmailRegex().IsMatch(value)
            ? Result<Email>.Success(new Email(value))
            : Result<Email>.Failure(EmailErrors.InvalidEmail);

    [GeneratedRegex(@"^\w+(\.\w+)*@\w+(\.\w+)+$", RegexOptions.Compiled)]
    private static partial Regex EmailRegex();
}
