namespace KluskaStore.Domain.Errors.ValueObjects;

using static Error;

public static class AddressErrors
{
    public static readonly Error MissingCountry = Validation("Address.MissingCountry", "The Country must not be empty");
    public static readonly Error MissingState = Validation("Address.MissingState", "The State must not be empty");
    public static readonly Error MissingCity = Validation("Address.MissingCity", "The City must not be empty");
    public static readonly Error MissingStreet = Validation("Address.MissingStreet", "The Street must not be empty");
}
