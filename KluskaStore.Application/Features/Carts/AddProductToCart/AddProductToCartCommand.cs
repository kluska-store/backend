namespace KluskaStore.Application.Features.Carts.AddProductToCart;

public sealed record AddProductToCartCommand(string SessionToken, Guid ProductId, uint Quantity) : IRequest<Result>;
