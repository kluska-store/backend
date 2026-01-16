using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Tests.Common.Builders;

public static class SessionOwnerBuilder
{
    private static SessionOwner Generate(bool isUser, Guid? ownerId = null) => new(
        isUser ? SessionOwner.OwnerTypeEnum.User : SessionOwner.OwnerTypeEnum.Store,
        ownerId ?? Guid.NewGuid()
    );

    public static SessionOwner Valid(bool isUser) => Generate(isUser);
    public static SessionOwner EmptyOwnerId(bool isUser) => Generate(isUser, Guid.Empty);
}
