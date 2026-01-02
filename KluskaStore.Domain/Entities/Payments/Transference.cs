using KluskaStore.Domain.Entities.Generics;
using KluskaStore.Domain.Errors.Entities;

namespace KluskaStore.Domain.Entities.Payments;

public sealed class Transference : AggregateRoot
{
    private Transference() { }

    internal Transference(Guid receiverStoreId, decimal value)
    {
        ReceiverStoreId = receiverStoreId;
        Value = value;
    }

    public Guid ReceiverStoreId { get; private set; }
    public decimal Value { get; private set; }

    public static Result<Transference> Create(Guid receiverStoreId, decimal value)
    {
        Error? error = null;
        if (receiverStoreId == Guid.Empty) error = TransferenceErrors.EmptyReceiverId;
        else if (value <= 0) error = TransferenceErrors.InvalidTransference;

        return error is null
            ? Result<Transference>.Success(new Transference(receiverStoreId, value))
            : Result<Transference>.Failure(error);
    }
}
