using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Application.Features.Common.Results;
using KluskaStore.Domain.Entities.Accounts;
using KluskaStore.Domain.ValueObjects.AccountData;

namespace KluskaStore.Application.Features.Users.CreateUser;

public sealed class CreateUserHandler(IUnitOfWork uow)
    : IRequestHandler<CreateUserCommand, Result<CreateUserResponse>>
{
    public async Task<Result<CreateUserResponse>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var cpfResult = Cpf.Create(request.Cpf);
        var emailResult = Email.Create(request.Email);
        var phoneResult = Phone.Create(request.Phone);

        var error = ResultHelper.FirstError(cpfResult, emailResult, phoneResult);
        if (error is not null) return Result<CreateUserResponse>.Failure(error);

        var userResult = User.Create(
            cpfResult.Value!,
            emailResult.Value!,
            request.Username,
            phoneResult.Value!,
            request.Birthday,
            request.RawPassword
        );

        if (userResult.IsFailure)
            return Result<CreateUserResponse>.Failure(userResult.Error!);

        var id = await uow.Users.AddAsync(userResult.Value!, cancellationToken);
        await uow.CommitAsync(cancellationToken);
        return Result<CreateUserResponse>.Success(new CreateUserResponse(id));
    }
}
