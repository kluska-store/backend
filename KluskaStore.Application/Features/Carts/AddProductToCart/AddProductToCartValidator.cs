using FluentValidation;

namespace KluskaStore.Application.Features.Carts.AddProductToCart;

public sealed class AddProductToCartValidator : AbstractValidator<AddProductToCartCommand>
{
    public AddProductToCartValidator()
    {
        RuleFor(c => c.SessionToken).NotEmpty();
        RuleFor(c => c.ProductId).NotEmpty();
        RuleFor(c => c.Quantity).NotEmpty();
    }
}
