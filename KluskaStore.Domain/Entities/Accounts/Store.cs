using KluskaStore.Domain.Entities.Generics;
using KluskaStore.Domain.ValueObjects.AccountData.Address;
using Cnpj = KluskaStore.Domain.ValueObjects.AccountData.Cnpj;
using Email = KluskaStore.Domain.ValueObjects.AccountData.Email;
using Phone = KluskaStore.Domain.ValueObjects.AccountData.Phone;
using static KluskaStore.Domain.Errors.Entities.StoreErrors;

namespace KluskaStore.Domain.Entities.Accounts;

public sealed class Store : AggregateRoot
{
    private readonly List<Phone> _phones;

    private Store()
    {
        Cnpj = null!;
        Name = null!;
        Email = null!;
        PasswordHash = null!;
        Address = null!;
        _phones = null!;
    }

    internal Store(Cnpj cnpj, string name, Email email, string passwordHash, Address address)
    {
        Cnpj = cnpj;
        Name = name;
        Email = email;
        IsActive = true;
        PasswordHash = passwordHash;
        Address = address;
        _phones = [];
    }

    public Cnpj Cnpj { get; private set; }
    public string Name { get; private set; }
    public string? PictureUrl { get; private set; }
    public Email Email { get; private set; }
    public bool IsActive { get; private set; }
    public string PasswordHash { get; private set; }
    public Address Address { get; private set; }
    public IReadOnlyList<Phone> Phones => _phones.AsReadOnly();

    public static Result<Store> Create(Cnpj cnpj, string name, Email email, string passwordHash, Address address)
    {
        Error? error = null;
        if (string.IsNullOrWhiteSpace(name)) error = EmptyName;
        else if (string.IsNullOrWhiteSpace(passwordHash)) error = EmptyPassword;

        return error is null
            ? Result<Store>.Success(new Store(cnpj, name, email, passwordHash, address))
            : Result<Store>.Failure(error);
    }

    public Result<Store> ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return Result<Store>.Failure(EmptyName);

        Name = name;
        return Result<Store>.Success(this);
    }

    public void ChangeEmail(Email email) => Email = email;

    public void ChangePicture(string? url) => PictureUrl = url;

    public Result<Store> ChangePasswordHash(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash)) return Result<Store>.Failure(EmptyPassword);

        PasswordHash = hash;
        return Result<Store>.Success(this);
    }

    public void ChangeAddress(Address address) => Address = address;

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;

    public void AddPhones(params IEnumerable<Phone> phones) => _phones.AddRange(phones);

    public void RemovePhone(Phone phone) => _phones.Remove(phone);
}
