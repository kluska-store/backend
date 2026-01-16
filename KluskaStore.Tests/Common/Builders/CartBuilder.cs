using KluskaStore.Domain.Entities.Products;

namespace KluskaStore.Tests.Common.Builders;

public static class CartBuilder
{
    public static Cart Valid() => new(Guid.NewGuid());
    public static Cart EmptyUserId() => new(Guid.Empty);
}
