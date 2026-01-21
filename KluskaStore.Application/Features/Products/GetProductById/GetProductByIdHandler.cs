using KluskaStore.Application.Abstractions.Persistence;
using static KluskaStore.Application.Features.Products.GetProductById.GetProductByIdErrors;

namespace KluskaStore.Application.Features.Products.GetProductById;

public class GetProductByIdHandler(IUnitOfWork uow)
    : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
{
    public async Task<Result<ProductDto>> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken = default
    )
    {
        var product = await uow.Products.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null) return Result<ProductDto>.Failure(ProductNotFound);
        return !product.IsAvailable
            ? Result<ProductDto>.Failure(ProductIsUnavailable)
            : Result<ProductDto>.Success(product.ToDto());
    }
}
