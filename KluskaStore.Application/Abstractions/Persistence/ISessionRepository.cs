using KluskaStore.Domain.Entities.Accounts;

namespace KluskaStore.Application.Abstractions.Persistence;

public interface ISessionRepository
{
    Task<string> RegisterAsync(User user, CancellationToken cancellationToken = default);
}
