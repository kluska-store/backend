namespace KluskaStore.Domain.Errors.ValueObjects;

using static Error;

public static class SessionOwnerErrors
{
    public static readonly Error EmptyOwnerId =
        Validation("SessionOwner.EmptyOwnerId", "The Owner's Id must not be empty");
}
