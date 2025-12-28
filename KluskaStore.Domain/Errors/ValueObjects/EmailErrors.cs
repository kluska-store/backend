namespace KluskaStore.Domain.Errors.ValueObjects;

using static Error;

public static class EmailErrors
{
    public static readonly Error InvalidEmail = Validation("Email.InvalidEmail", "Invalid Email");
}
