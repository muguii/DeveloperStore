namespace Sales.Domain.Abstractions;

public abstract class BaseEntity
{
    public Guid Id { get; private init; }

    protected BaseEntity()
    {

    }

    protected BaseEntity(Guid id) : this()
    {
        Id = id;
    }
}