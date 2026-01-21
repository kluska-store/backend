using KluskaStore.Application.Abstractions.Persistence;

namespace KluskaStore.Application.Features.Users.GetUserById;

public class GetUserByIdHandler(IUnitOfWork uow) : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken = default)
    {
        var user = await uow.Users.GetByIdAsync(request.UserId, cancellationToken);
        return user is not null
            ? Result<UserDto>.Success(user.ToDto())
            : Result<UserDto>.Failure(GetUserByIdErrors.NotFound);
    }
}
