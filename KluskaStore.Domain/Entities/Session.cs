using KluskaStore.Domain.Shared;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Domain.Entities;

public class Session : Entity<string>
{
    internal Session() { }

    private Session(SessionOwner owner, DateTime createdAt)
    {
        Owner = owner;
        CreatedAt = createdAt;
    }

    public SessionOwner Owner { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime ExpiresAt => CreatedAt.AddMonths(3);

    public bool IsExpired() => DateTime.UtcNow > ExpiresAt;

    private static Result<Session> Create(Result<SessionOwner> ownerResult, DateTime createdAt)
    {
        List<string> errors = [.. ownerResult.Errors];
        if (createdAt > DateTime.UtcNow) errors.Add("Session cannot be created in future");

        return errors.Count > 0
            ? Result<Session>.Failure(errors)
            : Result<Session>.Success(new Session(ownerResult.Value, createdAt));
    }

    public static Result<Session> CreateUserSession(Guid userId, DateTime createdAt) =>
        Create(SessionOwner.User(userId), createdAt);

    public static Result<Session> CreateStoreSession(Guid storeId, DateTime createdAt) =>
        Create(SessionOwner.Store(storeId), createdAt);
}
