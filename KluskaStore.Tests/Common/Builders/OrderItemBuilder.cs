using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Tests.Common.Builders;

public static class OrderItemBuilder
{
    private static OrderItem Generate(
        Guid? productId = null,
        string? name = null,
        decimal? unitPrice = null,
        uint? quantity = null
    ) => new(
        productId ?? Guid.NewGuid(),
        name ?? "product",
        unitPrice ?? 1m,
        "",
        quantity ?? 1u,
        new Dictionary<string, string>()
    );

    public static OrderItem Valid() => Generate();
    public static OrderItem EmptyProductId() => Generate(productId: Guid.Empty);
    public static OrderItem NoName() => Generate(name: "");
    public static OrderItem InvalidUnitPrice() => Generate(unitPrice: 0m);
    public static OrderItem Empty() => Generate(quantity: 0u);
}
