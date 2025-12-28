namespace KluskaStore.Domain.Shared;

public enum ErrorType
{
    NotFound,
    Validation,
    Conflict,
    Unauthorized,
    Forbidden,
    Unexpected
}
