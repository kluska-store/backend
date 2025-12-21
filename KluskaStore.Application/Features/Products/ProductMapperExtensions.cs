using KluskaStore.Application.Features.Products.GetProductById;
using KluskaStore.Domain.Entities.Products;

namespace KluskaStore.Application.Features.Products;

public static class ProductMapperExtensions
{
    private static readonly ProductMapper Mapper = new();

    public static ProductDto ToDto(this Product product) => Mapper.ToDto(product);
}
