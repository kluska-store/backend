using KluskaStore.Domain.Entities.Products;

namespace KluskaStore.Tests.Common.Builders;

public static class ProductBuilder
{
    private static Product Generate(decimal? price = null, string? name = null) => new(price ?? 1, name ?? "product");
    public static Product Valid() => Generate();
    public static Product InvalidPrice() => Generate(price: 0m);
    public static Product NoName() => Generate(name: "");

    public static Product Unavailable()
    {
        var product = Valid();
        product.MarkAsUnavailable();
        return product;
    }
}
