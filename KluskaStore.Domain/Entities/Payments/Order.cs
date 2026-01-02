using KluskaStore.Domain.Entities.Generics;
using KluskaStore.Domain.ValueObjects;
using static KluskaStore.Domain.Errors.Entities.OrderErrors;

namespace KluskaStore.Domain.Entities.Payments;

public sealed class Order : AggregateRoot
{
    public enum OrderStatusEnum
    {
        OrderReceived = 0,
        Processing = 1,
        Packing = 2,
        Shipped = 3,
        OutForDelivery = 4,
        Delivered = 5,
        Canceled = 6,
        Returned = 7
    }

    private readonly List<OrderItem> _items;

    private Order()
    {
        Status = OrderStatusEnum.OrderReceived;
        _items = null!;
    }

    internal Order(Guid userId, DateTime date, IEnumerable<OrderItem> items)
    {
        UserId = userId;
        Date = date;
        Status = OrderStatusEnum.OrderReceived;
        _items = items.ToList();
    }

    public Guid UserId { get; private set; }
    public DateTime Date { get; private set; }
    public OrderStatusEnum Status { get; private set; }

    public decimal TotalPrice => _items.Select(i => i.TotalPrice).Sum();
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
    public bool WasCanceled => Status == OrderStatusEnum.Canceled;
    public bool WasDelivered => Status == OrderStatusEnum.Delivered;
    public bool WasReturned => Status == OrderStatusEnum.Returned;
    public bool IsActive => Status is not (OrderStatusEnum.Canceled or OrderStatusEnum.Delivered);

    public static Result<Order> Create(Guid userId, DateTime date, IEnumerable<OrderItem> items)
    {
        var innerItems = items.ToList();

        Error? error = null;
        if (userId == Guid.Empty) error = EmptyUserId;
        else if (date > DateTime.UtcNow) error = InvalidOrderingDate;
        else if (innerItems.Count <= 0) error = EmptyOrder;

        return error is null
            ? Result<Order>.Success(new Order(userId, date, innerItems))
            : Result<Order>.Failure(error);
    }

    public void MarkAs(OrderStatusEnum status) => Status = status;
}
