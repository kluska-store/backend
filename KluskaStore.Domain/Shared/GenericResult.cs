namespace KluskaStore.Domain.Shared;

public class Result<T> : Result
{
    private Result(
        T value,
        bool isSucess,
        Error? error = null
    ) : base(isSucess, error) => Value = value;

    public T? Value { get; }

    public static Result<T> Success(T value) => new(value, true);

    public new static Result<T> Failure(Error error) => new(default!, false, error);
}
