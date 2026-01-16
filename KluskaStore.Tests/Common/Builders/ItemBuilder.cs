using KluskaStore.Domain.Entities.Products;

namespace KluskaStore.Tests.Common.Builders;

public static class ItemBuilder
{
    private static Item Generate(uint? quantity = null) => new(ProductBuilder.Valid(), quantity ?? 1);
    public static Item Valid() => Generate();
    public static Item Empty() => Generate(quantity: 0u);
}
