namespace KluskaStore.Domain.Errors.Entities;

using static Error;

public static class SessionErrors
{
    public static readonly Error EmptySessionToken =
        Validation("Session.EmptySessionToken", "The Session's Token must not be empty");

    public static readonly Error InvalidCreationDate =
        Validation("Session.InvalidCreationDate", "The Session's Creation Date must be placed in the past");
}
