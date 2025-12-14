namespace KluskaStore.Application.Features.Users.CreateUser;

public sealed record CreateUserCommand(
    string Cpf,
    string Email,
    string Username,
    string Phone,
    DateOnly Birthday,
    string RawPassword
) : IRequest<Result<CreateUserResponse>>;
