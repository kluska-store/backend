using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Domain.Entities.Accounts;
using Microsoft.EntityFrameworkCore;

namespace KluskaStore.Infrastructure.Persistence.Repositories;

public class SessionRepository(AppDbContext context) : ISessionRepository
{
    public async Task RegisterAsync<TSession>(TSession session, CancellationToken cancellationToken = default)
        where TSession : Session
    {
        switch (session)
        {
            case UserSession userSession:
                await context.UserSessions.AddAsync(userSession, cancellationToken);
                break;

            case StoreSession storeSession:
                // await context.StoreSessions.AddAsync(storeSession, cancellationToken);
                // break;
                throw new NotImplementedException();
        }
    }

    public void UnregisterAsync<TSession>(TSession session) where TSession : Session
    {
        switch (session)
        {
            case UserSession userSession:
                context.UserSessions.Remove(userSession);
                break;

            case StoreSession storeSession:
                // context.StoreSessions.Remove(storeSession);
                // break;
                throw new NotImplementedException();
        }
    }

    public async Task<Session?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var session =
            await context.UserSessions.FirstOrDefaultAsync(s => s.Token == token, cancellationToken);
        // ?? (Session?)await context.StoreSessions.FirstOrDefaultAsync(s => s.Token == token, cancellationToken);

        return session;
    }
}
