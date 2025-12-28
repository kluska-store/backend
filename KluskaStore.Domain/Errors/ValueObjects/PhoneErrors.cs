namespace KluskaStore.Domain.Errors.ValueObjects;

using static Error;

public static class PhoneErrors
{
    public static readonly Error InvalidPhone = Validation("Phone.InvalidPhone", "Invalid Phone");
}
