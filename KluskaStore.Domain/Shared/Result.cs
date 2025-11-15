namespace KluskaStore.Domain.Shared;

public class Result {
  private readonly List<string> _errors = [];
  public bool IsSuccess { get; }
  public bool IsFailure => !IsSuccess;
  public IReadOnlyList<string> Errors => _errors.AsReadOnly();

  protected Result(bool isSucess, params List<string> errors) {
    IsSuccess = isSucess;
    AddErrors(errors);
  }

  public static Result Success() => new(true);
  public static Result Failure(params List<string> errors) => new(false, errors);

  public void AddErrors(params List<string> errors) {
    _errors.AddRange(errors.Where(e => !string.IsNullOrWhiteSpace(e)));
  }
}