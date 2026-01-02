using KluskaStore.Domain.Errors.Entities;

namespace KluskaStore.Domain.Entities.Products;

public sealed class WishList : ProductCollection
{
    private WishList() => Name = null!;

    internal WishList(Guid userId, string name) : base(userId) => Name = name;
    public string Name { get; private set; }

    public static Result<WishList> Create(Guid userId, string name)
    {
        Error? error = null;
        if (userId == Guid.Empty) error = WishListErrors.EmptyUserId;
        else if (string.IsNullOrWhiteSpace(name)) error = WishListErrors.EmptyName;

        return error is null
            ? Result<WishList>.Success(new WishList(userId, name))
            : Result<WishList>.Failure(error);
    }
}
