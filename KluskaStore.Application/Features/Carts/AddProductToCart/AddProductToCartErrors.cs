namespace KluskaStore.Application.Features.Carts.AddProductToCart;

using static Error;

public static class AddProductToCartErrors
{
    public static readonly Error NotLoggedIn = Unauthorized("Cart.AddProduct.NotLoggedIn", "You need to log-in");

    public static readonly Error ProductNotAvailable =
        Conflict("Cart.AddProduct.ProductNotAvailable", "The Product is not currently available");

    public static readonly Error ProductNotFound =
        NotFound("Cart.AddProduct.ProductNotFound", "Could not find a Product with the specified Id");

    public static readonly Error NotAnUser = Forbidden("Cart.AddProduct.NotAnUser",
        "You need to be logged-in with an user account to do that");
}
