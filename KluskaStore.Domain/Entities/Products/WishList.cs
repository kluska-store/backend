using KluskaStore.Domain.Errors.Entities;

namespace KluskaStore.Domain.Entities.Products;

public class WishList : ProductCollection
{
    private WishList() => Name = null!;

    internal WishList(Guid userId, IEnumerable<Item> items, string name) : base(userId, items) => Name = name;
    public string Name { get; private set; }

    public static Result<WishList> Create(Guid userId, IEnumerable<Item> items, string name)
    {
        Error? error = null;
        if (userId == Guid.Empty) error = WishListErrors.EmptyUserId;
        else if (string.IsNullOrWhiteSpace(name)) error = WishListErrors.EmptyName;

        return error is null
            ? Result<WishList>.Success(new WishList(userId, items, name))
            : Result<WishList>.Failure(error);
    }
}
