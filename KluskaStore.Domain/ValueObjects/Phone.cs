using System.Text.RegularExpressions;
using KluskaStore.Domain.Shared;

namespace KluskaStore.Domain.ValueObjects;

public partial class Phone
{
    private Phone(string value) => Value = value;

    public string Value { get; }

    public static Result<Phone> Create(string value) =>
        PhoneRegex().IsMatch(value)
            ? Result<Phone>.Success(new Phone(value))
            : Result<Phone>.Failure("Invalid phone");

    [GeneratedRegex(@"^\+?\d{12,15}$")]
    private static partial Regex PhoneRegex();
}
