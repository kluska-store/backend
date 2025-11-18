using KluskaStore.Domain.Interfaces;

namespace KluskaStore.Domain.ValueObjects;

public abstract record ValueObject<T>(T Value) : IValueObject
{
    public override string ToString() => Value?.ToString() ?? "null";
}