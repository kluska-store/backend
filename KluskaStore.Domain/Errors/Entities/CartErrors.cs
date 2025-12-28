namespace KluskaStore.Domain.Errors.Entities;

using static Error;

public static class CartErrors
{
    public static readonly Error EmptyUserId = Validation("Cart.EmptyUserId", "The User Id must not be empty");
}
