using KluskaStore.Domain.Shared;

namespace KluskaStore.Domain.Entities.Products;

public class WishList : ProductCollection
{
    public string Name { get; private set; }

    private WishList() { }

    internal WishList(Guid userId, IEnumerable<Item> items, string name) : base(userId, items) => Name = name;

    public static Result<WishList> Create(Guid userId, IEnumerable<Item> items, string name)
    {
        List<string> errors = [];
        if (userId == Guid.Empty) errors.Add("User Id must valid");
        if (string.IsNullOrWhiteSpace(name)) errors.Add("Name must not be empty");

        return errors.Count == 0
            ? Result<WishList>.Success(new WishList(userId, items.Where(i => i.Quantity > 0), name))
            : Result<WishList>.Failure(errors);
    }
}
