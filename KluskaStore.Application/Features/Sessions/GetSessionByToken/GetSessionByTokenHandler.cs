using KluskaStore.Application.Abstractions.Persistence;

namespace KluskaStore.Application.Features.Sessions.GetSessionByToken;

public sealed class GetSessionByTokenHandler(ISessionRepository repository)
    : IRequestHandler<GetSessionByTokenQuery, GetSessionByTokenResponse?>
{
    public async Task<GetSessionByTokenResponse?> Handle(
        GetSessionByTokenQuery request,
        CancellationToken cancellationToken = default
    )
    {
        var session = await repository.GetByTokenAsync(request.SessionToken, cancellationToken);
        if (session is null) return null;
        return !session.IsExpired() ? session.ToResponse() : null;
    }
}
