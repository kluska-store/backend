using System.Text.RegularExpressions;
using KluskaStore.Domain.Errors.ValueObjects;

namespace KluskaStore.Domain.ValueObjects.AccountData;

public sealed partial record Phone
{
    internal Phone(string fullPhone) => FullPhone = fullPhone;

    public string FullPhone { get; }
    public string Ddi => GetPhonePart("ddi");
    public string Ddd => GetPhonePart("ddd");
    public string PhoneNumber => GetPhonePart("phone");


    public static Result<Phone> Create(string value) =>
        PhoneRegex().IsMatch(value)
            ? Result<Phone>.Success(new Phone(value))
            : Result<Phone>.Failure(PhoneErrors.InvalidPhone);

    private string GetPhonePart(string partName) => PhoneRegex().Match(FullPhone).Groups[partName].Value;

    [GeneratedRegex(@"^\+(?<ddi>\d{1,3}) \((?<ddd>\d{1,3})\) (?<phone>\d{4,5}-\d{4})$")]
    private static partial Regex PhoneRegex();
}
