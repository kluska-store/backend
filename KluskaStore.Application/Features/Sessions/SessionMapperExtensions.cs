using KluskaStore.Application.Features.Sessions.GetSessionByToken;
using KluskaStore.Domain.Entities.Accounts;

namespace KluskaStore.Application.Features.Sessions;

public static class SessionMapperExtensions
{
    private static readonly SessionMapper Mapper = new();

    public static GetSessionByTokenResponse ToResponse(this Session session) => Mapper.ToResponse(session);
}
