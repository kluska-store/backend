using KluskaStore.Domain.Entities.Generics;
using KluskaStore.Domain.ValueObjects.AccountData.Address;
using Cpf = KluskaStore.Domain.ValueObjects.AccountData.Cpf;
using Email = KluskaStore.Domain.ValueObjects.AccountData.Email;
using Phone = KluskaStore.Domain.ValueObjects.AccountData.Phone;
using static KluskaStore.Domain.Errors.Entities.UserErrors;

namespace KluskaStore.Domain.Entities.Accounts;

public class User : DefaultIdentityEntity
{
    private User()
    {
        Cpf = null!;
        Email = null!;
        Username = null!;
        Phone = null!;
        PasswordHash = null!;
        _addresses = null!;
    }

    internal User(
        Cpf cpf,
        Email email,
        string username,
        Phone phone,
        DateOnly birthday,
        string passwordHash
    )
    {
        Cpf = cpf;
        Email = email;
        Username = username;
        IsActive = true;
        Phone = phone;
        Birthday = birthday;
        PasswordHash = passwordHash;
        _addresses = [];
    }

    private readonly List<Address> _addresses;

    public Cpf Cpf { get; protected set; }
    public Email Email { get; protected set; }
    public string Username { get; protected set; }
    public string? ProfilePicture { get; protected set; }
    public bool IsActive { get; protected set; }
    public Phone Phone { get; protected set; }
    public DateOnly Birthday { get; protected set; }
    public string PasswordHash { get; protected set; }
    public IReadOnlyList<Address> Addresses => _addresses.AsReadOnly();

    public static Result<User> Create(
        Cpf cpf,
        Email email,
        string username,
        Phone phone,
        DateOnly birthday,
        string passwordHash
    )
    {
        Error? error = null;
        if (string.IsNullOrWhiteSpace(username)) error = EmptyUsername;
        else if (string.IsNullOrWhiteSpace(passwordHash)) error = EmptyPassword;
        else if (birthday.AddYears(18) > DateOnly.FromDateTime(DateTime.UtcNow)) error = InvalidBirthday;

        return error is null
            ? Result<User>.Success(new User(cpf, email, username, phone, birthday, passwordHash))
            : Result<User>.Failure(error);
    }

    public void ChangeEmail(Email newEmail) => Email = newEmail;

    public Result<User> ChangeUsername(string newUsername)
    {
        if (string.IsNullOrWhiteSpace(newUsername)) return Result<User>.Failure(EmptyUsername);

        Username = newUsername;
        return Result<User>.Success(this);
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;

    public void ChangePhone(Phone newPhone) => Phone = newPhone;

    public Result<User> ChangeBirthday(DateOnly newBirthday)
    {
        if (newBirthday.AddYears(18) > DateOnly.FromDateTime(DateTime.UtcNow))
            return Result<User>.Failure(InvalidBirthday);

        Birthday = newBirthday;
        return Result<User>.Success(this);
    }

    public Result<User> ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash)) return Result<User>.Failure(EmptyPassword);

        PasswordHash = newPasswordHash;
        return Result<User>.Success(this);
    }

    public void AddAddresses(params IEnumerable<Address> addresses) => _addresses.AddRange(addresses);

    public void RemoveAddresses(params IEnumerable<Address> addresses)
    {
        foreach (var address in addresses) _addresses.Remove(address);
    }

    public void RemoveAddressAt(int index) => _addresses.RemoveAt(index);

    public void ClearAddresses() => _addresses.Clear();

    public bool HasAnyAddress() => _addresses.Count > 0;
}
