using KluskaStore.Domain.Interfaces;

namespace KluskaStore.Domain.Entities;

public abstract class Entity<TId> : IEntity {
  public TId Id { get; protected set; }

  protected Entity() { }

  protected Entity(TId id) {
    Id = id;
  }

  public override bool Equals(object? obj) {
    if (obj is not Entity<TId> other) return false;
    if (ReferenceEquals(this, other)) return true;
    return EqualityComparer<TId>.Default.Equals(Id, other.Id);
  }

  public override int GetHashCode() {
    return EqualityComparer<TId>.Default.GetHashCode(Id!);
  }

  public static bool operator ==(Entity<TId>? a, Entity<TId>? b) {
    if (a is null && b is null) return true;
    if (a is null || b is null) return false;
    return a.Equals(b);
  }

  public static bool operator !=(Entity<TId>? a, Entity<TId>? b) {
    return !(a == b);
  }
}