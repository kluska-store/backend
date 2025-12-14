using KluskaStore.Application.Features.Users.GetUserById;
using KluskaStore.Domain.Entities.Accounts;
using KluskaStore.Domain.ValueObjects.AccountData;
using Riok.Mapperly.Abstractions;

namespace KluskaStore.Application.Features.Users;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class UserMapper
{
    public partial UserDto ToDto(User user);

    private static string MapCpf(Cpf cpf) => cpf.Value;
    private static string MapEmail(Email email) => email.Value;
    private static string MapPhone(Phone phone) => phone.FullPhone;
}
