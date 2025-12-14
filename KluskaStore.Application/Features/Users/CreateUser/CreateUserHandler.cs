using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Domain.Entities.Accounts;
using KluskaStore.Domain.ValueObjects.AccountData;

namespace KluskaStore.Application.Features.Users.CreateUser;

public sealed class CreateUserHandler(IUserRepository repository)
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

        var errors = cpfResult.Errors
            .Concat(emailResult.Errors)
            .Concat(phoneResult.Errors)
            .Distinct().ToList();

        if (errors.Count > 0) return Result<CreateUserResponse>.Failure(errors);

        var userResult = User.Create(
            cpfResult.Value,
            emailResult.Value,
            request.Username,
            phoneResult.Value,
            request.Birthday,
            request.RawPassword
        );

        errors = errors.Concat(userResult.Errors).Distinct().ToList();
        if (errors.Count > 0) return Result<CreateUserResponse>.Failure(errors);

        var id = await repository.AddAsync(userResult.Value, cancellationToken);
        return Result<CreateUserResponse>.Success(new CreateUserResponse(id));
    }
}
