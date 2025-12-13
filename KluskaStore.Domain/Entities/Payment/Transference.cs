using KluskaStore.Domain.Entities.Generics;
using KluskaStore.Domain.Shared;

namespace KluskaStore.Domain.Entities.Payment;

public class Transference : DefaultIdentityEntity
{
    public Guid ReceiverStoreId { get; private set; }
    public decimal Value { get; private set; }


    private Transference() { }

    internal Transference(Guid receiverStoreId, decimal value)
    {
        ReceiverStoreId = receiverStoreId;
        Value = value;
    }

    public static Result<Transference> Create(Guid receiverStoreId, decimal value)
    {
        List<string> errors = [];
        if (receiverStoreId == Guid.Empty) errors.Add("Receiver Store Id must not be empty");
        if (value <= 0) errors.Add("Value must be grater than 0");

        return errors.Count > 0
            ? Result<Transference>.Failure(errors)
            : Result<Transference>.Success(new Transference(receiverStoreId, value));
    }
}
