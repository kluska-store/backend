using KluskaStore.Domain.Entities.Generics;
using KluskaStore.Domain.Errors.Entities;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Domain.Entities.Accounts;

public class Session : Entity<string>
{
    private Session() => Owner = null!;

    internal Session(SessionOwner owner, DateTime createdAt)
    {
        Owner = owner;
        CreatedAt = createdAt;
    }

    public SessionOwner Owner { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime ExpiresAt => CreatedAt.AddMonths(3);

    public bool IsExpired() => DateTime.UtcNow > ExpiresAt;

    // TODO: separate SessionOwner logic from Session logic
    private static Result<Session> Create(Result<SessionOwner> ownerResult, DateTime createdAt)
    {
        if (ownerResult.IsFailure) return Result<Session>.Failure(ownerResult.Error!);
        return createdAt <= DateTime.UtcNow
            ? Result<Session>.Success(new Session(ownerResult.Value!, createdAt))
            : Result<Session>.Failure(SessionErrors.InvalidCreationDate);
    }

    public static Result<Session> CreateUserSession(Guid userId, DateTime createdAt) =>
        Create(SessionOwner.User(userId), createdAt);

    public static Result<Session> CreateStoreSession(Guid storeId, DateTime createdAt) =>
        Create(SessionOwner.Store(storeId), createdAt);
}
