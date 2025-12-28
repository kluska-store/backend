namespace KluskaStore.Domain.Errors.ValueObjects;

using static Error;

public class PostalCodeErrors
{
    public static readonly Error InvalidPostalCode = Validation("PostalCode.InvalidPostalCode", "Invalid postal code");
}
