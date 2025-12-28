using KluskaStore.Application.Abstractions.Persistence;
using static KluskaStore.Application.Features.Users.AuthenticateUser.AuthenticateUserErrors;

namespace KluskaStore.Application.Features.Users.AuthenticateUser;

// TODO: implement authentication through hashing
public sealed class AuthenticateUserHandler(IUnitOfWork uow) : IRequestHandler<AuthenticateUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await uow.Users.GetByEmailAsync(request.Email, cancellationToken);
            if (user is null) return Result<string>.Failure(UserNotFound);
            if (user.PasswordHash != request.Password) return Result<string>.Failure(PasswordIsIncorect);

            var sessionToken = await uow.Sessions.RegisterAsync(user, cancellationToken);
            await uow.CommitAsync(cancellationToken);
            return Result<string>.Success(sessionToken);
        }
        catch
        {
            await uow.RollbackAsync(cancellationToken);
            return Result<string>.Failure(UnexpectedError);
        }
    }
}
