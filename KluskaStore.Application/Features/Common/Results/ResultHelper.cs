namespace KluskaStore.Application.Features.Common.Results;

public static class ResultHelper
{
    public static Error? FirsError(params Result[] results) => results
        .Where(r => r.IsFailure)
        .Select(r => r.Error)
        .FirstOrDefault();
}
