using KluskaStore.Domain.Entities.Generics;
using KluskaStore.Domain.Errors.Entities;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Domain.Entities.Accounts;

public sealed class Session : Entity
{
    private Session()
    {
        Token = "";
        Owner = null!;
    }

    internal Session(string token, SessionOwner owner, DateTime createdAt)
    {
        Token = token;
        Owner = owner;
        CreatedAt = createdAt;
    }

    public string Token { get; private set; }
    public SessionOwner Owner { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; internal set; }

    public bool IsExpired() => DateTime.UtcNow > ExpiresAt && ExpiresAt != default;

    public static Result<Session> Create(string token, SessionOwner sessionOwner, DateTime createdAt)
    {
        Error? error = null;
        if (createdAt > DateTime.UtcNow) error = SessionErrors.InvalidCreationDate;
        else if (string.IsNullOrEmpty(token)) error = SessionErrors.EmptySessionToken;

        return error is not null
            ? Result<Session>.Failure(error)
            : Result<Session>.Success(new Session(token, sessionOwner, createdAt));
    }

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
