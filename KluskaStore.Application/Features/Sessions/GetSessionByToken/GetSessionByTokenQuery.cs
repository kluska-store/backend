using KluskaStore.Domain.Entities.Accounts;

namespace KluskaStore.Application.Features.Sessions.GetSessionByToken;

public sealed record GetSessionByTokenQuery(string SessionToken) : IRequest<GetSessionByTokenResponse?>;
