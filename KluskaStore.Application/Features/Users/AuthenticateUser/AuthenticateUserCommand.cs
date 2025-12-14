namespace KluskaStore.Application.Features.Users.AuthenticateUser;

public sealed record AuthenticateUserCommand(string Email, string Password) : IRequest<Result<string>>;
