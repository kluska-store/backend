using KluskaStore.Domain.Interfaces;
using KluskaStore.Domain.Shared;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Domain.Entities;

public class Order : Entity<uint>
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

    public Guid UserId { get; private set; }
    public DateTime Date { get; private set; }
    public OrderStatusEnum Status { get; private set; }

    public decimal TotalPrice => _items.Select(i => i.TotalPrice).Sum();
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
    public bool WasCanceled => Status == OrderStatusEnum.Canceled;
    public bool WasDelivered => Status == OrderStatusEnum.Delivered;
    public bool WasReturned => Status == OrderStatusEnum.Returned;
    public bool IsActive => Status is not (OrderStatusEnum.Canceled or OrderStatusEnum.Delivered);

    private Order() { }

    internal Order(Guid userId, DateTime date, IEnumerable<OrderItem> items)
    {
        UserId = userId;
        Date = date;
        Status = OrderStatusEnum.OrderReceived;
        _items = items.ToList();
    }

    public static Result<Order> Create(Guid userId, DateTime date, IEnumerable<OrderItem> items)
    {
        var innerItems = items.ToList();

        List<string> errors = [];
        if (userId == Guid.Empty) errors.Add("User Id must not be empty");
        if (date > DateTime.UtcNow) errors.Add("Ordering Date cannot be in the future");
        if (innerItems.Count <= 0) errors.Add("Every Order must include at least 1 Item");

        return errors.Count > 0
            ? Result<Order>.Failure(errors)
            : Result<Order>.Success(new Order(userId, date, innerItems));
    }

    public void MarkAs(OrderStatusEnum status) => Status = status;
}
