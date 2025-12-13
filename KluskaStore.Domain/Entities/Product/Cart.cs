using KluskaStore.Domain.Shared;

namespace KluskaStore.Domain.Entities.Product;

public class Cart : ProductCollection
{
    private Cart() { }

    internal Cart(Guid userId, IEnumerable<Item> items) : base(userId, items) { }

    public static Result<Cart> Create(Guid userId, IEnumerable<Item> items) =>
        userId == Guid.Empty
            ? Result<Cart>.Failure("User Id must be valid")
            : Result<Cart>.Success(new Cart(userId, items.Where(i => i.Quantity > 0)));
}
