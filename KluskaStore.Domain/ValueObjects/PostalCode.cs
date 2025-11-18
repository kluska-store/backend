using System.Text.RegularExpressions;
using KluskaStore.Domain.Shared;

namespace KluskaStore.Domain.ValueObjects;

public partial record PostalCode : ValueObject<string>
{
    private PostalCode(string value) : base(value) { }
    
    public static Result<PostalCode> Create(string value) =>
        PostalCodeRegex().IsMatch(value)
            ? Result<PostalCode>.Success(new PostalCode(value))
            : Result<PostalCode>.Failure("Invalid postal code");

    [GeneratedRegex(@"\d{8}")]
    private static partial Regex PostalCodeRegex();
}