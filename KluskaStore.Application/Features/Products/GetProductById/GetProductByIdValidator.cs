using FluentValidation;

namespace KluskaStore.Application.Features.Products.GetProductById;

public class GetProductByIdValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdValidator() => RuleFor(q => q.ProductId).NotEmpty();
}
