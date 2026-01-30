using KluskaStore.Domain.Entities.Accounts;

namespace KluskaStore.Tests.Common.Builders;

using static KluskaStore.Domain.Entities.Accounts.User;

public static class UserBuilder
{
    private static User Generate(string? username = null, DateOnly? birthday = null, string? passwordHash = null) =>
        new(
            CpfBuilder.Valid(),
            EmailBuilder.Valid(),
            username ?? "username",
            PhoneBuilder.Valid(),
            birthday ?? DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-18)),
            passwordHash ?? "password"
        );

    public static User Valid() => Generate();

    public static User WithId(Guid id)
    {
        var user = Valid();
        user.Id = id;
        return user;
    }

    public static User Underage() => Generate(birthday: DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-15)));
    public static User NoUsername() => Generate(username: "");
    public static User NoPassword() => Generate(passwordHash: "");
}
