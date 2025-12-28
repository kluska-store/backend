using KluskaStore.Domain.Errors.Entities;

namespace KluskaStore.Domain.Entities.Products;

public class Cart : ProductCollection
{
    private Cart() { }

    internal Cart(Guid userId, IEnumerable<Item> items) : base(userId, items) { }

    public static Result<Cart> Create(Guid userId, IEnumerable<Item> items) => userId != Guid.Empty
        ? Result<Cart>.Success(new Cart(userId, items))
        : Result<Cart>.Failure(CartErrors.EmptyUserId);
}
