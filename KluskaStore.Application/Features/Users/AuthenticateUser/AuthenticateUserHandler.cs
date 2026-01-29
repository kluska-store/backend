using KluskaStore.Application.Abstractions.Persistence;
using static KluskaStore.Application.Features.Users.AuthenticateUser.AuthenticateUserErrors;

namespace KluskaStore.Application.Features.Users.AuthenticateUser;

// TODO: implement authentication through hashing
public sealed class AuthenticateUserHandler(
    IUnitOfWork uow,
    IUserRepository userRepo,
    ISessionRepository sessionRepo
) : IRequestHandler<AuthenticateUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(
        AuthenticateUserCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var user = await userRepo.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null) return Result<string>.Failure(UserNotFound);
        if (user.PasswordHash != request.Password) return Result<string>.Failure(PasswordIsIncorrect);

        var sessionToken = await sessionRepo.RegisterAsync(user.Id, cancellationToken);
        await uow.CommitAsync(cancellationToken);
        return Result<string>.Success(sessionToken);
    }
}
