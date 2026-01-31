using KluskaStore.Application.Abstractions.Persistence;

namespace KluskaStore.Infrastructure.Persistence;

//TODO: protect classes
public sealed class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken = default)
        => await context.SaveChangesAsync(cancellationToken);
}
