using System.Text.RegularExpressions;
using KluskaStore.Domain.Interfaces;
using KluskaStore.Domain.Shared;

namespace KluskaStore.Domain.ValueObjects.PersonalData;

public partial class Email : IValueObject
{
    internal Email(string value) => Value = value;

    public string Value { get; }

    public static Result<Email> Create(string value) =>
        EmailRegex().IsMatch(value)
            ? Result<Email>.Success(new Email(value))
            : Result<Email>.Failure("Invalid email");

    [GeneratedRegex(@"^\w+(\.\w+)*@\w+(\.\w+)+$", RegexOptions.Compiled)]
    private static partial Regex EmailRegex();
}
