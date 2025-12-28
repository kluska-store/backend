namespace KluskaStore.Domain.Errors.Entities;

using static Error;

public static class UserErrors
{
    public static readonly Error EmptyUsername = Validation("User.EmptyUsername", "The Username must not be empty");
    public static readonly Error EmptyPassword = Validation("User.EmptyPassword", "The Password must not be empty");

    public static readonly Error InvalidBirthday =
        Validation("User.InvalidBirthday", "The user must be at least 18 years old");
}
