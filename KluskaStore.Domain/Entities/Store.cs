using KluskaStore.Domain.Shared;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Domain.Entities;

public class Store : Entity<Guid>
{
    private Store() { }

    private Store(Cnpj cnpj, string name, Email email, string passwordHash)
    {
        Cnpj = cnpj;
        Name = name;
        Email = email;
        IsActive = true;
        PasswordHash = passwordHash;
    }

    public Cnpj Cnpj { get; private set; }
    public string Name { get; private set; }
    public string? PictureUrl { get; private set; }
    public Email Email { get; private set; }
    public bool IsActive { get; private set; }
    public string PasswordHash { get; private set; }

    public static Result<Store> Create(Cnpj cnpj, string name, Email email, string passwordHash)
    {
        List<string> errors = [];

        if (string.IsNullOrWhiteSpace(name)) errors.Add("Name must not be empty");
        if (string.IsNullOrWhiteSpace(passwordHash)) errors.Add("Password must not be empty");

        return errors.Count == 0
            ? Result<Store>.Success(new Store(cnpj, name, email, passwordHash))
            : Result<Store>.Failure(errors);
    }

    public Result<Store> ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return Result<Store>.Failure("Name must not be empty");

        Name = name;
        return Result<Store>.Success(this);
    }

    public void ChangeEmail(Email email)
    {
        Email = email;
    }

    public void ChangePicture(string? url)
    {
        PictureUrl = url;
    }

    public Result<Store> ChangePasswordHash(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash)) return Result<Store>.Failure("Password must not be empty");

        PasswordHash = hash;
        return Result<Store>.Success(this);
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}