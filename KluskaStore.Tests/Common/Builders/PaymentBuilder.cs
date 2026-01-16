using KluskaStore.Domain.Entities.Payments;

namespace KluskaStore.Tests.Common.Builders;

public static class PaymentBuilder
{
    private static Payment Generate(
        Guid? payerUserId = null,
        DateTime? date = null,
        Guid? orderId = null,
        IEnumerable<Transference>? transferences = null
    ) => new(
        payerUserId ?? Guid.NewGuid(),
        date ?? DateTime.UtcNow,
        orderId ?? Guid.NewGuid(),
        transferences ?? new List<Transference> { TransferenceBuilder.Valid() }
    );

    public static Payment Valid() => Generate();
    public static Payment EmptyPayerId() => Generate(payerUserId: Guid.Empty);
    public static Payment InvalidDate() => Generate(date: DateTime.UtcNow.AddDays(1));
    public static Payment EmptyOrderId() => Generate(orderId: Guid.Empty);
    public static Payment NoTransferences() => Generate(transferences: []);
}
