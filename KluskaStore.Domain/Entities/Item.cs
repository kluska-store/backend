using KluskaStore.Domain.Entities.Generics;
using KluskaStore.Domain.Shared;

namespace KluskaStore.Domain.Entities;

public class Item : DefaultIdentityEntity
{
    public Product Product { get; private set; }
    public uint Quantity { get; internal set; }

    private Item() { }

    internal Item(Product product, uint quantity)
    {
        Product = product;
        Quantity = quantity;
    }

    public static Result<Item> Create(Product product, uint quantity) =>
        quantity == 0
            ? Result<Item>.Failure("Items must be created with quantity >= 1")
            : Result<Item>.Success(new Item(product, quantity));
}
