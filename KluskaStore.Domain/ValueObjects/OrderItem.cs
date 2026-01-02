using static KluskaStore.Domain.Errors.ValueObjects.OrderItemErrors;

namespace KluskaStore.Domain.ValueObjects;

public sealed record OrderItem
{
    internal OrderItem(
        Guid productId,
        string name,
        decimal unitPrice,
        string? description,
        uint quantity,
        IEnumerable<KeyValuePair<string, string>> specifications
    )
    {
        ProductId = productId;
        Name = name;
        UnitPrice = unitPrice;
        Description = description;
        Quantity = quantity;
        Specifications = specifications.ToDictionary();
    }

    public Guid ProductId { get; }
    public string Name { get; }
    public decimal UnitPrice { get; }
    public string? Description { get; }
    public uint Quantity { get; }
    public decimal TotalPrice => UnitPrice * Quantity;
    public Dictionary<string, string> Specifications { get; }

    public static Result<OrderItem> Create(
        Guid productId,
        string name,
        decimal unitPrice,
        string? description,
        uint quantity,
        IEnumerable<KeyValuePair<string, string>> specifications
    )
    {
        Error? error = null;
        if (productId == Guid.Empty) error = EmptyProductId;
        else if (string.IsNullOrWhiteSpace(name)) error = EmptyName;
        else if (unitPrice <= 0) error = InvalidUnitPrice;
        else if (quantity <= 0) error = InvalidQuantity;

        return error is not null
            ? Result<OrderItem>.Failure(error)
            : Result<OrderItem>.Success(
                new OrderItem(productId, name, unitPrice, description, quantity, specifications)
            );
    }
}
