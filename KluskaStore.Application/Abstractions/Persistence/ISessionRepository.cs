using KluskaStore.Domain.Entities.Accounts;

namespace KluskaStore.Application.Abstractions.Persistence;

public interface ISessionRepository
{
    Task RegisterAsync(Session session, CancellationToken cancellationToken = default);
    void UnregisterAsync(Session session);
    Task<Session?> GetByTokenAsync(string sessionToken, CancellationToken cancellationToken = default);
}
