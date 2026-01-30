namespace KluskaStore.Application.Features.Sessions.EndSession;

using static Error;

public static class EndSessionErrors
{
    public static readonly Error SessionNotFound = NotFound(
        "Session.End.NotFound",
        "We weren't able to locate the target Session in order to delete it"
    );
}
