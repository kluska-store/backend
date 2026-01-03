using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Domain.Entities.Products;
using static KluskaStore.Application.Features.Carts.AddProductToCart.AddProductToCartErrors;

namespace KluskaStore.Application.Features.Carts.AddProductToCart;

public sealed class AddProductToCartHandler(IUnitOfWork uow) : IRequestHandler<AddProductToCartCommand, Result>
{
    public async Task<Result> Handle(
        AddProductToCartCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var session = await uow.Sessions.GetByTokenAsync(request.SessionToken, cancellationToken);
        if (session is null || session.IsExpired()) return Result.Failure(NotLoggedIn);

        if (!session.Owner.IsUser) return Result.Failure(NotAnUser);
        var userId = session.Owner.OwnerId;

        var product = await uow.Products.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null) return Result.Failure(ProductNotFound);
        if (!product.IsAvailable) return Result.Failure(ProductNotAvailable);

        var itemResult = Item.Create(product, request.Quantity);
        if (itemResult.IsFailure) return Result.Failure(itemResult.Error!);
        var item = itemResult.Value!;

        var cart = await uow.Carts.GetByUserIdAsync(userId, cancellationToken);
        if (cart is null)
        {
            var cartResult = Cart.Create(userId);
            if (cartResult.IsFailure) return Result.Failure(cartResult.Error!);
            cart = cartResult.Value!;
            await uow.Carts.AddAsync(cart, cancellationToken);
        }

        cart.AddItem(item);

        await uow.CommitAsync(cancellationToken);
        return Result.Success();
    }
}
