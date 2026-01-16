using KluskaStore.Domain.Entities.Products;

namespace KluskaStore.Tests.Common.Builders;

public static class WishListBuilder
{
    private static WishList Generate(Guid? userId = null, string? name = null) =>
        new(userId ?? Guid.NewGuid(), name ?? "wish list");

    public static WishList Valid() => Generate();
    public static WishList EmptyUserId() => Generate(userId: Guid.Empty);
    public static WishList NoName() => Generate(name: "");
}
