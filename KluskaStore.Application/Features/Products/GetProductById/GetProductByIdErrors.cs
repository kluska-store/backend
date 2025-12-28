namespace KluskaStore.Application.Features.Products.GetProductById;

using static Error;

public static class GetProductByIdErrors
{
    public static readonly Error ProductNotFound =
        NotFound("Product.GetById.NotFound", "Failed to locate a Product with the given Id");

    public static readonly Error ProductIsUnavailable =
        Conflict("Product.GetById.Unavailable", "The Product is currently unavailable");
}
