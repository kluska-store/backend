using KluskaStore.Domain.Entities.Accounts;

namespace KluskaStore.Application.Abstractions.Persistence;

public interface ISessionRepository
{
    Task RegisterAsync<TSession>(TSession session, CancellationToken cancellationToken = default)
        where TSession : Session;

    void UnregisterAsync<TSession>(TSession session) where TSession : Session;

    Task<Session?> GetByTokenAsync(string sessionToken, CancellationToken cancellationToken = default);
}
