namespace KluskaStore.Application.Exceptions;

public class NotFoundException : ApplicationException {
  private NotFoundException(string message) : base(message) { }

  public static NotFoundException Store(Guid id) => new($"Store with id {id} not found");
}