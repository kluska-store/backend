using KluskaStore.Domain.Entities.Accounts;

namespace KluskaStore.Application.Abstractions.Persistence;

public interface ISessionRepository
{
    Task<string> RegisterAsync(Session session, CancellationToken cancellationToken = default);
    Task UnregisterAsync(string sessionToken, CancellationToken cancellationToken = default);
    Task<Session?> GetByTokenAsync(string sessionToken, CancellationToken cancellationToken = default);
}
