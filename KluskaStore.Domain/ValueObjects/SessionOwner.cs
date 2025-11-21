using KluskaStore.Domain.Shared;

namespace KluskaStore.Domain.ValueObjects;

public class SessionOwner
{
    public enum OwnerTypeEnum
    {
        User = 1,
        Store = 2
    }

    private SessionOwner(OwnerTypeEnum type, Guid ownerId)
    {
        OwnerType = type;
        OwnerId = ownerId;
    }

    public OwnerTypeEnum OwnerType { get; }
    public Guid OwnerId { get; }

    private static Result<SessionOwner> Create(OwnerTypeEnum type, Guid ownerId) =>
        ownerId == Guid.Empty
            ? Result<SessionOwner>.Failure("Invalid session owner id")
            : Result<SessionOwner>.Success(new SessionOwner(type, ownerId));

    public static Result<SessionOwner> User(Guid userId) => Create(OwnerTypeEnum.User, userId);

    public static Result<SessionOwner> Store(Guid storeId) => Create(OwnerTypeEnum.Store, storeId);
}
