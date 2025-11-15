namespace KluskaStore.Domain.Shared;

public class Result<T> : Result {
  public T Value { get; }
  
  private Result(T value, bool isSuccess, params List<string> errors) : base(isSuccess, errors) {
    Value = value;
  }

  public static Result<T> Success(T value) => new(value, true);
  public new static Result<T> Failure(params List<string> errors) => new(default!, false, errors);
}