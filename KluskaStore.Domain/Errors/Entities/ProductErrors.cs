namespace KluskaStore.Domain.Errors.Entities;

using static Error;

public static class ProductErrors
{
    public static readonly Error InvalidPrice = Validation("Product.InvalidPrice", "The Price must be grater than 0");
    public static readonly Error EmptyName = Validation("Product.EmptyName", "The Name must not be empty");
}
