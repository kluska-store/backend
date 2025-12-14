using FluentValidation;

namespace KluskaStore.Application.Features.Sessions.EndSession;

public class EndSessionValidator : AbstractValidator<EndSessionCommand>
{
    public EndSessionValidator() => RuleFor(c => c.SessionToken).NotEmpty();
}
