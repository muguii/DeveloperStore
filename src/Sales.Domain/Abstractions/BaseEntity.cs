namespace Sales.Domain.Abstractions;

public abstract class BaseEntity
{
    private readonly List<DomainEvent> _domainEvents = [];

    public Guid Id { get; private init; }
    public List<DomainEvent> DomainEvents => _domainEvents.ToList();

    protected BaseEntity()
    {

    }

    protected BaseEntity(Guid id) : this()
    {
        Id = id;
    }

    protected void QueueDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}