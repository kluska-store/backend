using KluskaStore.Application.Abstractions.Persistence;
using static KluskaStore.Application.Features.Carts.RemoveItemFromCart.RemoveItemFromCartErrors;

namespace KluskaStore.Application.Features.Carts.RemoveItemFromCart;

public sealed class RemoveItemFromCartHandler(
    IUnitOfWork uow,
    ISessionRepository sessionRepo,
    ICartRepository cartRepo,
    IItemRepository itemRepo
) : IRequestHandler<RemoveItemFromCartCommand, Result>
{
    public async Task<Result> Handle(RemoveItemFromCartCommand request, CancellationToken cancellationToken = default)
    {
        var session = await sessionRepo.GetByTokenAsync(request.SessionToken, cancellationToken);
        if (session is null || session.IsExpired()) return Result.Failure(NotLoggedIn);
        if (session.Owner.IsStore) return Result.Failure(NotAnUser);
        var userId = session.Owner.OwnerId;

        var cart = await cartRepo.GetByUserIdAsync(userId, cancellationToken);
        if (cart is null) return Result.Failure(CartNotFound);

        var item = await itemRepo.GetByIdAsync(request.ItemId, cancellationToken);
        if (item is null) return Result.Failure(ItemNotFound);

        if (!cart.RemoveItem(item)) return Result.Failure(ItemNotInCart);

        await uow.CommitAsync(cancellationToken);
        return Result.Success();
    }
}
