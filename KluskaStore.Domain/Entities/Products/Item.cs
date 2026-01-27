using KluskaStore.Domain.Entities.Generics;
using KluskaStore.Domain.Errors.Entities;

namespace KluskaStore.Domain.Entities.Products;

public sealed class Item : Entity
{
    private Item() => Product = null!;

    internal Item(Product product, uint quantity)
    {
        Product = product;
        Quantity = quantity;
    }

    public Guid Id { get; private set; }
    public Product Product { get; private set; }
    public uint Quantity { get; internal set; }

    public static Result<Item> Create(Product product, uint quantity) => quantity > 0
        ? Result<Item>.Success(new Item(product, quantity))
        : Result<Item>.Failure(ItemErrors.EmptyItem);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Product.Id;
    }
}
