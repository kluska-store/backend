using KluskaStore.Domain.Entities.Accounts;

namespace KluskaStore.Application.Abstractions.Persistence;

public interface IUserRepository
{
    Task<Guid> AddAsync(User user, CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
