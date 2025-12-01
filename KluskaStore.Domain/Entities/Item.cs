using KluskaStore.Domain.Shared;

namespace KluskaStore.Domain.Entities;

public class Item : Entity<uint>
{
    public Product Product { get; private set; }
    public uint Quantity { get; private set; }

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

    public bool ChangeQuantity(uint newQuantity)
    {
        Quantity = newQuantity;
        return Quantity > 0;
    }
}
