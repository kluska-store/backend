using KluskaStore.Domain.Entities.Generics;

namespace KluskaStore.Domain.Entities.Products;

public class Item : DefaultIdentityEntity
{
    private Item() { }

    internal Item(Product product, uint quantity)
    {
        Product = product;
        Quantity = quantity;
    }

    public Product Product { get; private set; }
    public uint Quantity { get; internal set; }

    public static Result<Item> Create(Product product, uint quantity) =>
        quantity == 0
            ? Result<Item>.Failure("Items must be created with quantity >= 1")
            : Result<Item>.Success(new Item(product, quantity));
}
