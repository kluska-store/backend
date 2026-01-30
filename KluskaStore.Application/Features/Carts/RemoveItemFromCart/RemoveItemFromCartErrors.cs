namespace KluskaStore.Application.Features.Carts.RemoveItemFromCart;

using static Error;

public static class RemoveItemFromCartErrors
{
    public static readonly Error ItemNotInCart =
        NotFound("Cart.RemoveItem.ItemNotInCart", "The Item was not in the Cart, so it was not removed");

    public static readonly Error CartNotFound =
        NotFound("Cart.RemoveItem.CartNotFound", "We weren't able to locate a Cart to remove the Item from");

    public static readonly Error NotAnUser =
        Forbidden("Cart.RemoveItem.NotAnUser", "You need to be logged in with an User account to do this");

    public static readonly Error NotLoggedIn =
        Unauthorized("Cart.RemoveItem.NotLoggedIn", "You need to be logged in to to this");
}
