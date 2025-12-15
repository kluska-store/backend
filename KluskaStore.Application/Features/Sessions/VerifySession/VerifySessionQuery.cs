namespace KluskaStore.Application.Features.Sessions.VerifySession;

public sealed record VerifySessionQuery(string SessionToken) : IRequest<bool>;
