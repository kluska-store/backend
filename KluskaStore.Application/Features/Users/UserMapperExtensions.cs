using KluskaStore.Application.Features.Users.GetUserById;
using KluskaStore.Domain.Entities.Accounts;

namespace KluskaStore.Application.Features.Users;

public static class UserMapperExtensions
{
    private static readonly UserMapper Mapper = new();

    public static UserDto ToDto(this User user) => Mapper.ToDto(user);
}
