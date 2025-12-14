using KluskaStore.Application.Abstractions.Persistence;

namespace KluskaStore.Application.Features.Sessions.EndSession;

public class EndSessionHandler(ISessionRepository repository) : IRequestHandler<EndSessionCommand>
{
    public async Task Handle(EndSessionCommand request, CancellationToken cancellationToken = default)
        => await repository.UnregisterAsync(request.SessionToken, cancellationToken);
}
