namespace KluskaStore.Domain.Shared;

public class Result
{
    private readonly List<string> _errors = [];

    protected Result(bool isSucess, params List<string> errors)
    {
        IsSuccess = isSucess;
        AddErrors(errors);
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IReadOnlyList<string> Errors => _errors.AsReadOnly();

    public static Result Success() => new(true);

    public static Result Failure(params List<string> errors) => new(false, errors);

    public void AddErrors(params List<string> errors)
    {
        _errors.AddRange(errors.Where(e => !string.IsNullOrWhiteSpace(e)));
    }
}