namespace KluskaStore.Application.Features.Carts.RemoveItemFromCart;

public sealed record RemoveItemFromCartCommand(string SessionToken, Guid ItemId) : IRequest<Result>;
