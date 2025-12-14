namespace KluskaStore.Application.Features.Sessions.EndSession;

public record EndSessionCommand(string SessionToken) : IRequest;
