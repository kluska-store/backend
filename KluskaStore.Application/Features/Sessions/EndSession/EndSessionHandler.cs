using KluskaStore.Application.Abstractions.Persistence;

namespace KluskaStore.Application.Features.Sessions.EndSession;

public class EndSessionHandler(IUnitOfWork uow, ISessionRepository sessionRepo)
    : IRequestHandler<EndSessionCommand, Result>
{
    public async Task<Result> Handle(EndSessionCommand request, CancellationToken cancellationToken = default)
    {
        var session = await sessionRepo.GetByTokenAsync(request.SessionToken, cancellationToken);
        if (session is null) return Result.Failure(EndSessionErrors.SessionNotFound);

        sessionRepo.UnregisterAsync(session);
        await uow.CommitAsync(cancellationToken);
        return Result.Success();
    }
}
