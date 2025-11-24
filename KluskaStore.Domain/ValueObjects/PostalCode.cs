using System.Text.RegularExpressions;
using KluskaStore.Domain.Shared;
using KluskaStore.Domain.Interfaces;

namespace KluskaStore.Domain.ValueObjects;

public partial class PostalCode : IValueObject
{
    internal PostalCode(string value) => Value = value;

    public string Value { get; }

    public static Result<PostalCode> Create(string value) =>
        PostalCodeRegex().IsMatch(value)
            ? Result<PostalCode>.Success(new PostalCode(value))
            : Result<PostalCode>.Failure("Invalid postal code");

    [GeneratedRegex(@"^\d{8}$")]
    private static partial Regex PostalCodeRegex();
}
