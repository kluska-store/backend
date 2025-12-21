namespace KluskaStore.Application.Features.Products.GetProductById;

public record GetProductByIdQuery(Guid ProductId) : IRequest<Result<ProductDto>>;
