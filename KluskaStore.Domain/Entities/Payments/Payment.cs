using KluskaStore.Domain.Entities.Generics;
using KluskaStore.Domain.Shared;

namespace KluskaStore.Domain.Entities.Payments;

public class Payment : DefaultIdentityEntity
{
    private readonly List<Transference> _transferences;

    public Guid PayerUserId { get; private set; }
    public DateTime Date { get; private set; }
    public Guid OrderId { get; private set; }

    public IReadOnlyList<Transference> Transferences => _transferences.AsReadOnly();
    public decimal Value => _transferences.Select(t => t.Value).Sum();

    private Payment() { }

    internal Payment(Guid payerUserId, DateTime date, Guid orderId, IEnumerable<Transference> transfererences)
    {
        PayerUserId = payerUserId;
        Date = date;
        OrderId = orderId;
        _transferences = transfererences.ToList();
    }

    public static Result<Payment> Create(Guid payerUserId, DateTime date, Guid orderId, IEnumerable<Transference> transferences)
    {
        var innerTransferences = transferences.ToList();
        List<string> errors = [];
        if (payerUserId == Guid.Empty) errors.Add("Payer User Id must not be empty");
        if (date > DateTime.UtcNow) errors.Add("Payment Date must not be in the future");
        if (orderId == Guid.Empty) errors.Add("Order Id must not be empty");
        if (innerTransferences.Count < 1) errors.Add("A Payment must contain at least 1 Transference");

        return errors.Count > 0
            ? Result<Payment>.Failure(errors)
            : Result<Payment>.Success(new Payment(payerUserId, date, orderId, innerTransferences));
    }
}
