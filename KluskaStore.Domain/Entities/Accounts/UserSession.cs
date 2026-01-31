using KluskaStore.Domain.Errors.Entities;

namespace KluskaStore.Domain.Entities.Accounts;

public sealed class UserSession : Session
{
    private UserSession() { }

    internal UserSession(string token, Guid userId, DateTime createdAt) : base(token, createdAt) => UserId = userId;

    public Guid UserId { get; private set; }


    public static Result<UserSession> Create(string token, Guid userId, DateTime createdAt)
    {
        Error? error = null;
        if (createdAt > DateTime.UtcNow) error = SessionErrors.InvalidCreationDate;
        else if (string.IsNullOrEmpty(token)) error = SessionErrors.EmptySessionToken;
        else if (userId == Guid.Empty) error = SessionErrors.EmptyOwnerId;

        return error is not null
            ? Result<UserSession>.Failure(error)
            : Result<UserSession>.Success(new UserSession(token, userId, createdAt));
    }
}
