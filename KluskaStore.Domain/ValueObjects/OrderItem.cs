using KluskaStore.Domain.Interfaces;

namespace KluskaStore.Domain.ValueObjects;

public record OrderItem : IValueObject
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
        List<string> errors = [];
        if (productId == Guid.Empty) errors.Add("Product Id must not be empty");
        if (string.IsNullOrWhiteSpace(name)) errors.Add("Name must not be empty");
        if (unitPrice <= 0) errors.Add("Unit Price must be grater than 0");
        if (quantity <= 0) errors.Add("Quantity must be grater than 0");

        return errors.Count > 0
            ? Result<OrderItem>.Failure(errors)
            : Result<OrderItem>.Success(
                new OrderItem(productId, name, unitPrice, description, quantity, specifications)
            );
    }
}
