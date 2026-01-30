using KluskaStore.Application.Abstractions;
using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Domain.Entities.Accounts;
using KluskaStore.Domain.ValueObjects;
using static KluskaStore.Application.Features.Users.AuthenticateUser.AuthenticateUserErrors;

namespace KluskaStore.Application.Features.Users.AuthenticateUser;

// TODO: implement authentication through hashing
public sealed class AuthenticateUserHandler(
    IUnitOfWork uow,
    IUserRepository userRepo,
    ISessionRepository sessionRepo,
    ISessionTokenGenerator tokenGenerator
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

        var sessionOwnerResult = SessionOwner.User(user.Id);
        if (sessionOwnerResult.IsFailure) return Result<string>.Failure(sessionOwnerResult.Error!);

        var sessionResult = Session.Create(tokenGenerator.New(), sessionOwnerResult.Value!, DateTime.UtcNow);
        if (sessionResult.IsFailure) return Result<string>.Failure(sessionResult.Error!);
        var session = sessionResult.Value!;

        await sessionRepo.RegisterAsync(session, cancellationToken);
        await uow.CommitAsync(cancellationToken);
        return Result<string>.Success(session.Token);
    }
}
