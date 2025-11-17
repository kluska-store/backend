using KluskaStore.Domain.Interfaces;

namespace KluskaStore.Domain.ValueObjects;

public abstract class ValueObject<T>(T value) : IValueObject
{
    public readonly T Value = value;

    public sealed override bool Equals(object? obj)
    {
        if (obj is not ValueObject<T> other) return false;
        if (ReferenceEquals(this, other)) return true;
        return EqualityComparer<T>.Default.Equals(Value, other.Value);
    }

    public sealed override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Value!);
    
    public static bool operator ==(ValueObject<T>? a, ValueObject<T>? b) => a?.Equals(b) ?? b is null;
    
    public static bool operator !=(ValueObject<T>? a, ValueObject<T>? b) => !(a == b);
}