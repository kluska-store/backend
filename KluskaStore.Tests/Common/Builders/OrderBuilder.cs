using KluskaStore.Domain.Entities.Payments;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Tests.Common.Builders;

public static class OrderBuilder
{
    private static Order Generate(
        Guid? userId = null,
        DateTime? date = null,
        IEnumerable<OrderItem>? items = null
    ) => new(
        userId ?? Guid.NewGuid(),
        date ?? DateTime.UtcNow,
        items ?? new List<OrderItem> { OrderItemBuilder.Valid() }
    );

    public static Order Valid() => Generate();
    public static Order EmptyUserId() => Generate(userId: Guid.Empty);
    public static Order InvalidDate() => Generate(date: DateTime.UtcNow.AddDays(1));
    public static Order Empty() => Generate(items: []);
}
