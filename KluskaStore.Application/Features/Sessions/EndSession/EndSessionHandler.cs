using KluskaStore.Application.Abstractions.Persistence;

namespace KluskaStore.Application.Features.Sessions.EndSession;

public class EndSessionHandler(IUnitOfWork uow) : IRequestHandler<EndSessionCommand>
{
    public async Task Handle(EndSessionCommand request, CancellationToken cancellationToken = default)
    {
        await uow.Sessions.UnregisterAsync(request.SessionToken, cancellationToken);
        await uow.CommitAsync(cancellationToken);
    }
}
