using FluentValidation;

namespace KluskaStore.Application.Features.Sessions.GetSessionByToken;

public sealed class GetSessionByTokenValidator : AbstractValidator<GetSessionByTokenQuery>
{
    public GetSessionByTokenValidator() => RuleFor(q => q.SessionToken).NotEmpty();
}
