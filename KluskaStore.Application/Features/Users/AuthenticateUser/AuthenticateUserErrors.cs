namespace KluskaStore.Application.Features.Users.AuthenticateUser;

using static Error;

public static class AuthenticateUserErrors
{
    public static readonly Error UnexpectedError = Unexpected("User.Authenticate.Unexpected", "Something went wrong");

    public static readonly Error UserNotFound =
        NotFound("User.Authenticate.NotFound", "Failed to locate User with given Id");

    public static readonly Error PasswordIsIncorect =
        Unauthorized("User.Authentication.IncorrectPassword", "The Password is incorrect");
}
