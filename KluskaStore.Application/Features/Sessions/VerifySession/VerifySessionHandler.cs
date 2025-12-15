using KluskaStore.Application.Abstractions.Persistence;

namespace KluskaStore.Application.Features.Sessions.VerifySession;

public class VerifySessionHandler(ISessionRepository repository) : IRequestHandler<VerifySessionQuery, bool>
{
    public async Task<bool> Handle(VerifySessionQuery request, CancellationToken cancellationToken = default)
    {
        var session = await repository.GetByTokenAsync(request.SessionToken, cancellationToken);
        return !session?.IsExpired() ?? false;
    }
}
