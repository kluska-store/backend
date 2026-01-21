using KluskaStore.Domain.Entities.Products;

namespace KluskaStore.Tests.Common.Builders;

public static class CartBuilder
{
    public static Cart Valid() => new(Guid.NewGuid());
    public static Cart WithUserId(Guid userId) => new(userId);
    public static Cart EmptyUserId() => new(Guid.Empty);
}
