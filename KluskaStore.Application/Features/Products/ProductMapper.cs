using KluskaStore.Application.Features.Products.GetProductById;
using KluskaStore.Domain.Entities.Products;
using Riok.Mapperly.Abstractions;

namespace KluskaStore.Application.Features.Products;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProductMapper
{
    public partial ProductDto ToDto(Product product);
}
