namespace KluskaStore.Application.Features.Users.AuthenticateUser;

using static Error;

public static class AuthenticateUserErrors
{
    public static readonly Error UserNotFound =
        NotFound("User.Authenticate.NotFound", "Failed to locate User with given Id");

    public static readonly Error PasswordIsIncorrect =
        Unauthorized("User.Authentication.IncorrectPassword", "The Password is incorrect");
}
