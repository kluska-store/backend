using System.Text.RegularExpressions;
using KluskaStore.Domain.Errors.ValueObjects;

namespace KluskaStore.Domain.ValueObjects.AccountData.Address;

public sealed partial record PostalCode
{
    internal PostalCode(string value) => Value = value;

    public string Value { get; }

    public static Result<PostalCode> Create(string value) =>
        PostalCodeRegex().IsMatch(value)
            ? Result<PostalCode>.Success(new PostalCode(value))
            : Result<PostalCode>.Failure(PostalCodeErrors.InvalidPostalCode);

    [GeneratedRegex(@"^\d{8}$")]
    private static partial Regex PostalCodeRegex();
}
