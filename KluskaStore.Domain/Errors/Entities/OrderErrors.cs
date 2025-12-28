namespace KluskaStore.Domain.Errors.Entities;

using static Error;

public static class OrderErrors
{
    public static readonly Error EmptyUserId = Validation("Order.EmptyUserId", "The User's Id must not be empty");
    public static readonly Error EmptyOrder = Validation("Order.EmptyOrder", "The Order must contain at least 1 item");

    public static readonly Error InvalidOrderingDate =
        Validation("Order.InvalidOrderingDate", "The Ordering Date must be placed in the past");
}
