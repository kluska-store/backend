using KluskaStore.Domain.Entities.Generics;

namespace KluskaStore.Domain.Entities.Payments;

public class Transference : DefaultIdentityEntity
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
        List<string> errors = [];
        if (receiverStoreId == Guid.Empty) errors.Add("Receiver Store Id must not be empty");
        if (value <= 0) errors.Add("Value must be grater than 0");

        return errors.Count > 0
            ? Result<Transference>.Failure(errors)
            : Result<Transference>.Success(new Transference(receiverStoreId, value));
    }
}
