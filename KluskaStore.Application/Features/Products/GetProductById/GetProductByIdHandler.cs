using KluskaStore.Application.Abstractions.Persistence;

namespace KluskaStore.Application.Features.Products.GetProductById;

public class GetProductByIdHandler(IProductRepository repository)
    : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
{
    public async Task<Result<ProductDto>> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken = default
    )
    {
        var product = await repository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null) return Result<ProductDto>.Failure("Product not found");
        return !product.IsAvailable
            ? Result<ProductDto>.Failure("The Product is unavailable")
            : Result<ProductDto>.Success(product.ToDto());
    }
}
