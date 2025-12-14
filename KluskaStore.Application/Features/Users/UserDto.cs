using KluskaStore.Domain.ValueObjects.AccountData.Address;

namespace KluskaStore.Application.Features.Users;

public sealed record UserDto(
    Guid Id,
    string Cpf,
    string Email,
    string Username,
    string? ProfilePicture,
    bool IsActive,
    string Phone,
    DateOnly Birthday,
    List<Address> Addresses
);
