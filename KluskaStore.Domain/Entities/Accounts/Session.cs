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

    public static Result<Session> Create(SessionOwner sessionOwner, DateTime createdAt) =>
        createdAt <= DateTime.UtcNow
            ? Result<Session>.Success(new Session(sessionOwner, createdAt))
            : Result<Session>.Failure(SessionErrors.InvalidCreationDate);
}
