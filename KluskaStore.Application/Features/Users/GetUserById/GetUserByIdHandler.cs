using KluskaStore.Application.Abstractions.Persistence;

namespace KluskaStore.Application.Features.Users.GetUserById;

public class GetUserByIdHandler(IUserRepository repository) : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken = default)
    {
        var user = await repository.GetByIdAsync(request.UserId, cancellationToken);
        return user?.ToDto();
    }
}
