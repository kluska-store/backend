namespace KluskaStore.Application.Features.Common.Results;

public static class ResultHelper
{
    public static Error? FirstError(params Result[] results) => results
        .Where(r => r.IsFailure)
        .Select(r => r.Error)
        .FirstOrDefault();
}
