using KluskaStore.Domain.Entities.Generics;
using KluskaStore.Domain.Errors.Entities;

namespace KluskaStore.Domain.Entities.Accounts;

public abstract class Session : Entity
{
    protected Session() => Token = "";

    internal Session(string token, DateTime createdAt)
    {
        Token = token;
        CreatedAt = createdAt;
    }

    public string Token { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime ExpiresAt { get; internal set; }

    public bool IsExpired() => DateTime.UtcNow > ExpiresAt && ExpiresAt != default;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Token;
    }

    public Result<Session> SetExpirationDate(DateTime expirationDate)
    {
        if (expirationDate <= DateTime.UtcNow)
            return Result<Session>.Failure(SessionErrors.InvalidExpirationDate);

        ExpiresAt = expirationDate;
        return Result<Session>.Success(this);
    }
}
