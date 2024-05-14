namespace Evently.Common.Domain;

public abstract class Entity
{
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    private readonly List<IDomainEvent> _domainEvents = [];

    protected Entity() { }
    
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
    
    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}