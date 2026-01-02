namespace KluskaStore.Domain.Entities.Generics;

public abstract class AggregateRoot() : Entity
{
    public Guid Id { get; protected set; } = Guid.Empty;

    protected AggregateRoot(Guid id) : this() => Id = id;

    protected sealed override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
    }
}
