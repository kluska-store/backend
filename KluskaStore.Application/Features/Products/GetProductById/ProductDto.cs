namespace KluskaStore.Application.Features.Products.GetProductById;

public sealed record ProductDto(
    Guid Id,
    string Name,
    decimal Price,
    string? Description,
    IReadOnlyDictionary<string, string> Specifications
);
