using KluskaStore.Domain.Entities.Payments;

namespace KluskaStore.Tests.Common.Builders;

public static class TransferenceBuilder
{
    private static Transference Generate(Guid? receiverStoreId = null, decimal? value = null) =>
        new(receiverStoreId ?? Guid.NewGuid(), value ?? 1m);

    public static Transference Valid() => Generate();
    public static Transference EmptyReceiverId() => Generate(receiverStoreId: Guid.Empty);
    public static Transference InvalidValue() => Generate(value: 0m);
}
