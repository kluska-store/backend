using FluentValidation;

namespace KluskaStore.Application.Features.Sessions.VerifySession;

public sealed class VerifySessionValidator : AbstractValidator<VerifySessionQuery>
{
    public VerifySessionValidator() => RuleFor(q => q.SessionToken).NotEmpty();
}
