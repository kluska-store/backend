namespace KluskaStore.Domain.Errors.ValueObjects;

using static Error;

public static class OrderItemErrors
{
    public static readonly Error EmptyProductId =
        Validation("OrderItem.EmptyProductId", "The Product's Id must not be empty");

    public static readonly Error EmptyName = Validation("OrderItem.EmptyName", "The Item's Name must not be empty");

    public static readonly Error InvalidUnitPrice =
        Validation("OrderItem.InvalidUnitPrice", "The Item's Unit Price must be grater than 0");

    public static readonly Error InvalidQuantity =
        Validation("OrderItem.InvalidQuantity", "The Item's Quantity must be grater than 0");
}
