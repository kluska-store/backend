using FluentValidation;

namespace KluskaStore.Application.Features.Carts.RemoveItemFromCart;

public sealed class RemoveItemFromCartValidator : AbstractValidator<RemoveItemFromCartCommand>
{
    public RemoveItemFromCartValidator()
    {
        RuleFor(c => c.ItemId).NotEmpty();
        RuleFor(c => c.SessionToken).NotEmpty();
    }
}
