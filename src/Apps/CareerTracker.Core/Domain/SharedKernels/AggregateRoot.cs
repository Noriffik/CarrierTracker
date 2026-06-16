namespace CareerTracker.Core.Domain.SharedKernels;

public abstract class ValueObject;

public abstract class AggregateRoot<T> : Entity<T>
{
    protected AggregateRoot(T id) : base(id) { }
}
