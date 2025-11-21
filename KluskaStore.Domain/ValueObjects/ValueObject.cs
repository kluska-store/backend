using KluskaStore.Domain.Interfaces;

namespace KluskaStore.Domain.ValueObjects;

public abstract class ValueObject<T>(T value) : IValueObject
{
    public T Value { get; } = value;
    public override string ToString() => Value is null ? "null" : Value.ToString()!;
}
