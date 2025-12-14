using FluentValidation;

namespace KluskaStore.Application.Features.Users.GetUserById;

public sealed class GetUserByIdValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdValidator() => RuleFor(q => q.UserId).NotEmpty();
}
