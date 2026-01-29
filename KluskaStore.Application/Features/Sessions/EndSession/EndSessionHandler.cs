using KluskaStore.Application.Abstractions.Persistence;

namespace KluskaStore.Application.Features.Sessions.EndSession;

public class EndSessionHandler(IUnitOfWork uow, ISessionRepository sessionRepo)
    : IRequestHandler<EndSessionCommand, Result>
{
    public async Task<Result> Handle(EndSessionCommand request, CancellationToken cancellationToken = default)
    {
        await sessionRepo.UnregisterAsync(request.SessionToken, cancellationToken);
        await uow.CommitAsync(cancellationToken);
        return Result.Success();
    }
}
