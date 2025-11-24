using KluskaStore.Domain.Shared;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Domain.Entities;

public class Store : Entity<Guid>
{
    private Store() { }

    internal Store(Cnpj cnpj, string name, Email email, string passwordHash, Address address, params IEnumerable<Phone> phones)
    {
        Cnpj = cnpj;
        Name = name;
        Email = email;
        IsActive = true;
        PasswordHash = passwordHash;
        Address = address;
        _phones = phones.ToList();
    }

    private readonly List<Phone> _phones;

    public Cnpj Cnpj { get; private set; }
    public string Name { get; private set; }
    public string? PictureUrl { get; private set; }
    public Email Email { get; private set; }
    public bool IsActive { get; private set; }
    public string PasswordHash { get; private set; }
    public Address Address { get; private set; }
    public IReadOnlyList<Phone> Phones => _phones.AsReadOnly();

    public static Result<Store> Create(Cnpj cnpj, string name, Email email, string passwordHash, Address address, params IEnumerable<Phone> phones)
    {
        List<string> errors = [];

        if (string.IsNullOrWhiteSpace(name)) errors.Add("Name must not be empty");
        if (string.IsNullOrWhiteSpace(passwordHash)) errors.Add("Password must not be empty");

        return errors.Count == 0
            ? Result<Store>.Success(new Store(cnpj, name, email, passwordHash, address, phones))
            : Result<Store>.Failure(errors);
    }

    public Result<Store> ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return Result<Store>.Failure("Name must not be empty");

        Name = name;
        return Result<Store>.Success(this);
    }

    public void ChangeEmail(Email email) => Email = email;

    public void ChangePicture(string? url) => PictureUrl = url;

    public Result<Store> ChangePasswordHash(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash)) return Result<Store>.Failure("Password must not be empty");

        PasswordHash = hash;
        return Result<Store>.Success(this);
    }

    public void ChangeAddress(Address address) => Address = address;

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;

    public void AddPhones(params IEnumerable<Phone> phones) => _phones.AddRange(phones);

    public void RemovePhoneAt(Phone phone) => _phones.Remove(phone);
}
