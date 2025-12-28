using KluskaStore.Domain.Entities.Generics;

namespace KluskaStore.Domain.Entities.Products;

public abstract class ProductCollection : DefaultIdentityEntity
{
    protected readonly List<Item> _items;

    protected ProductCollection() => _items = null!;

    internal ProductCollection(Guid userId, IEnumerable<Item> items)
    {
        UserId = userId;
        _items = items.ToList();
    }

    public Guid UserId { get; protected set; }
    public IReadOnlyList<Item> Items => _items.AsReadOnly();

    public bool SetItem(Product product, uint quantity)
    {
        var item = _items.Find(i => i.Product == product);

        if (item is not null && quantity == 0) _items.Remove(item);
        else if (item is not null) item.Quantity = quantity;
        else if (quantity != 0)
        {
            var newItem = new Item(product, quantity);
            _items.Add(newItem);
        }

        return quantity != 0;
    }

    public void RemoveItem(Product product)
    {
        var item = _items.Find(i => i.Product == product);
        if (item is not null) _items.Remove(item);
    }

    public void AddItem(Product product)
    {
        var item = _items.Find(i => i.Product == product);

        if (item is not null) item.Quantity += 1;
        else _items.Add(new Item(product, 1));
    }

    public decimal CalculateTotalPrice() => _items.Select(i => i.Product.Price * i.Quantity).Sum();
}
