namespace KluskaStore.Application.Features.Users.GetUserById;

public static class GetUserByIdErrors
{
    public static readonly Error NotFound =
        Error.NotFound("User.GetById.NotFound", "We could not find an User with the given Id");
}
