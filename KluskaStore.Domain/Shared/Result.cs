namespace KluskaStore.Domain.Shared;

public class Result
{
    protected Result(bool isSucess, Error? error = null)
    {
        IsSuccess = isSucess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public Error? Error { get; }
    public bool IsFailure => !IsSuccess;

    public static Result Success() => new(true);

    public static Result Failure(Error error) => new(false, error);
}
