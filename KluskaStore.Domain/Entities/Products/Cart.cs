using KluskaStore.Domain.Errors.Entities;

namespace KluskaStore.Domain.Entities.Products;

public sealed class Cart : ProductCollection
{
    private Cart() { }

    internal Cart(Guid userId) : base(userId) { }

    public static Result<Cart> Create(Guid userId) => userId != Guid.Empty
        ? Result<Cart>.Success(new Cart(userId))
        : Result<Cart>.Failure(CartErrors.EmptyUserId);
}
