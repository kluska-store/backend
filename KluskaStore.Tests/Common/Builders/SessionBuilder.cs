using KluskaStore.Domain.Entities.Accounts;

namespace KluskaStore.Tests.Common.Builders;

public static class SessionBuilder
{
    private static Session Generate(bool isUserSession, string? token = null, DateTime? creationDate = null) => new(
        token ?? "session token",
        SessionOwnerBuilder.Valid(isUser: isUserSession),
        creationDate ?? DateTime.UtcNow
    );

    public static Session Valid(bool isUserSession) => Generate(isUserSession);
    public static Session EmptyToken(bool isUserSession) => Generate(isUserSession, token: "");
    public static Session Expired(bool isUserSession) => Generate(isUserSession, creationDate: DateTime.UtcNow.AddYears(-2));

    public static Session InvalidCreationDate(bool isUserSession) =>
        Generate(isUserSession, creationDate: DateTime.UtcNow.AddDays(1));
}
