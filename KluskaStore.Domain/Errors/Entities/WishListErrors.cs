namespace KluskaStore.Domain.Errors.Entities;

using static Error;

public static class WishListErrors
{
    public static readonly Error EmptyUserId = Validation("WishList.EmptyUserId", "The User's Id must not be empty");
    public static readonly Error EmptyName = Validation("WishList.EmptyName", "The Name must not be empty");
}
