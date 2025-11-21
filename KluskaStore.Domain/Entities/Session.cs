using KluskaStore.Domain.Shared;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Domain.Entities;

public class Session : Entity<string>
{
    private Session(SessionOwner owner, DateTime createdAt)
    {
        Owner = owner;
        CreatedAt = createdAt;
    }

    public SessionOwner Owner { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime ExpiresAt => CreatedAt.AddMonths(3);

    public bool IsExpired() => DateTime.UtcNow > ExpiresAt;

    private static Result<Session> Create(Result<SessionOwner> ownerResult, DateTime createdAt) =>
        ownerResult.IsFailure
            ? Result<Session>.Failure(ownerResult.Errors)
            : Result<Session>.Success(new Session(ownerResult.Value, createdAt));
}
