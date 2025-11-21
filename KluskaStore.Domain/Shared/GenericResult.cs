namespace KluskaStore.Domain.Shared;

public class Result<T> : Result
{
    private Result(T value, bool isSuccess, params List<string> errors) : base(isSuccess, errors) => Value = value;

    public T Value { get; }

    public static Result<T> Success(T value) => new(value, true);

    public new static Result<T> Failure(params List<string> errors) => new(default!, false, errors);
}
