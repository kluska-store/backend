using KluskaStore.Domain.Entities.Generics;
using static KluskaStore.Domain.Errors.Entities.PaymentErrors;

namespace KluskaStore.Domain.Entities.Payments;

public sealed class Payment : AggregateRoot
{
    private readonly List<Transference> _transferences;

    private Payment() => _transferences = null!;

    internal Payment(Guid payerUserId, DateTime date, Guid orderId, IEnumerable<Transference> transfererences)
    {
        PayerUserId = payerUserId;
        Date = date;
        OrderId = orderId;
        _transferences = transfererences.ToList();
    }

    public Guid PayerUserId { get; private set; }
    public DateTime Date { get; private set; }
    public Guid OrderId { get; private set; }

    public IReadOnlyList<Transference> Transferences => _transferences.AsReadOnly();
    public decimal Value => _transferences.Select(t => t.Value).Sum();

    public static Result<Payment> Create(Guid payerUserId, DateTime date, Guid orderId,
        IEnumerable<Transference> transferences)
    {
        var innerTransferences = transferences.ToList();
        Error? error = null;
        if (payerUserId == Guid.Empty) error = EmptyUserId;
        else if (date > DateTime.UtcNow) error = InvalidPaymentDate;
        else if (orderId == Guid.Empty) error = EmptyOrderId;
        else if (innerTransferences.Count < 1) error = EmptyPayment;

        return error is null
            ? Result<Payment>.Success(new Payment(payerUserId, date, orderId, innerTransferences))
            : Result<Payment>.Failure(error);
    }
}
