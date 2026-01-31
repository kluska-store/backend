using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Domain.Entities.Accounts;
using KluskaStore.Domain.Entities.Products;
using static KluskaStore.Application.Features.Carts.AddProductToCart.AddProductToCartErrors;

namespace KluskaStore.Application.Features.Carts.AddProductToCart;

public sealed class AddProductToCartHandler(
    IUnitOfWork uow,
    ISessionRepository sessionRepo,
    IProductRepository productRepo,
    ICartRepository cartRepo
) : IRequestHandler<AddProductToCartCommand, Result>
{
    public async Task<Result> Handle(
        AddProductToCartCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var session = await sessionRepo.GetByTokenAsync(request.SessionToken, cancellationToken);
        if (session is null || session.IsExpired()) return Result.Failure(NotLoggedIn);

        if (session is not UserSession userSession) return Result.Failure(NotAnUser);
        var userId = userSession.UserId;

        var product = await productRepo.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null) return Result.Failure(ProductNotFound);
        if (!product.IsAvailable) return Result.Failure(ProductNotAvailable);

        var itemResult = Item.Create(product, request.Quantity);
        if (itemResult.IsFailure) return Result.Failure(itemResult.Error!);
        var item = itemResult.Value!;

        var cart = await cartRepo.GetByUserIdAsync(userId, cancellationToken);
        if (cart is null)
        {
            var cartResult = Cart.Create(userId);
            if (cartResult.IsFailure) return Result.Failure(cartResult.Error!);
            cart = cartResult.Value!;
            await cartRepo.AddAsync(cart, cancellationToken);
        }

        cart.AddItem(item);

        await uow.CommitAsync(cancellationToken);
        return Result.Success();
    }
}
