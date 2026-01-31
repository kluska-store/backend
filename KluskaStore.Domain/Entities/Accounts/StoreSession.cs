using KluskaStore.Domain.Errors.Entities;

namespace KluskaStore.Domain.Entities.Accounts;

public sealed class StoreSession : Session
{
    private StoreSession() { }

    internal StoreSession(string token, Guid storeId, DateTime createdAt) : base(token, createdAt) => StoreId = storeId;

    public Guid StoreId { get; private set; }

    public static Result<StoreSession> Create(string token, Guid storeId, DateTime createdAt)
    {
        Error? error = null;
        if (createdAt > DateTime.UtcNow) error = SessionErrors.InvalidCreationDate;
        else if (string.IsNullOrEmpty(token)) error = SessionErrors.EmptySessionToken;
        else if (storeId == Guid.Empty) error = SessionErrors.EmptyOwnerId;

        return error is not null
            ? Result<StoreSession>.Failure(error)
            : Result<StoreSession>.Success(new StoreSession(token, storeId, createdAt));
    }
}
