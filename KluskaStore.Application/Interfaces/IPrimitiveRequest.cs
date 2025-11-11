namespace KluskaStore.Application.Interfaces;

public interface IPrimitiveRequest<T> : IDto where T : struct { }