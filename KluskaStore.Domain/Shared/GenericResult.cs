namespace KluskaStore.Domain.Shared;

public class Result<T> : Result
{
    internal Result(T value, bool isSuccess, params IEnumerable<string> errors) : base(isSuccess, errors) => Value = value;

    public T Value { get; }

    public static Result<T> Success(T value) => new(value, true);

    public new static Result<T> Failure(params IEnumerable<string> errors) => new(default!, false, errors);
}
