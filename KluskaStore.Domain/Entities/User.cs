using KluskaStore.Domain.Shared;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Domain.Entities;

public class User : Entity<Guid>
{
    private User() { }

    internal User(Cpf cpf, Email email, string username, Phone phone, DateOnly birthday, string passwordHash)
    {
        Cpf = cpf;
        Email = email;
        Username = username;
        IsActive = true;
        Phone = phone;
        Birthday = birthday;
        PasswordHash = passwordHash;
    }

    public Cpf Cpf { get; protected set; }
    public Email Email { get; protected set; }
    public string Username { get; protected set; }
    public string? ProfilePicture { get; protected set; }
    public bool IsActive { get; protected set; }
    public Phone Phone { get; protected set; }
    public DateOnly Birthday { get; protected set; }
    public string PasswordHash { get; protected set; }

    public static Result<User> Create(
        Cpf cpf,
        Email email,
        string username,
        Phone phone,
        DateOnly birthday,
        string passwordHash
    )
    {
        List<string> errors = [];
        if (string.IsNullOrWhiteSpace(username)) errors.Add("Username must not be empty");
        if (string.IsNullOrWhiteSpace(passwordHash)) errors.Add("Password must not be empty");
        if (birthday.AddYears(18) > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            errors.Add("User must be 18 or more years old");
        }

        if (errors.Count > 0) return Result<User>.Failure(errors);

        var user = new User(cpf, email, username, phone, birthday, passwordHash);
        return Result<User>.Success(user);
    }

    public void ChangeEmail(Email newEmail) => Email = newEmail;

    public Result<User> ChangeUsername(string newUsername)
    {
        if (string.IsNullOrWhiteSpace(newUsername)) return Result<User>.Failure("Username must not be empty");

        Username = newUsername;
        return Result<User>.Success(this);
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;

    public void ChangePhone(Phone newPhone) => Phone = newPhone;

    public Result<User> ChangeBirthday(DateOnly newBirthday)
    {
        if (newBirthday.AddYears(18) > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            return Result<User>.Failure("User must be 18 or more years old");
        }

        Birthday = newBirthday;
        return Result<User>.Success(this);
    }

    public Result<User> ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash)) return Result<User>.Failure("Password must not be empty");

        PasswordHash = newPasswordHash;
        return Result<User>.Success(this);
    }
}
