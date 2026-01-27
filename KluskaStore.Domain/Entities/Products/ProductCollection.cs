using KluskaStore.Domain.Entities.Generics;

namespace KluskaStore.Domain.Entities.Products;

public abstract class ProductCollection : AggregateRoot
{
    protected readonly HashSet<Item> _items = [];

    protected ProductCollection() => _items = null!;

    internal ProductCollection(Guid userId) => UserId = userId;

    public Guid UserId { get; protected set; }
    public IReadOnlySet<Item> Items => _items;

    public bool RemoveItem(Item item) => _items.Remove(item);

    public void AddItem(Item item)
    {
        if (_items.Add(item)) return;
        var stored = _items.First(i => i.Product.Id == item.Product.Id);
        stored.Quantity += item.Quantity;
    }

    public decimal CalculateTotalPrice() => _items.Select(i => i.Product.Price * i.Quantity).Sum();
}
