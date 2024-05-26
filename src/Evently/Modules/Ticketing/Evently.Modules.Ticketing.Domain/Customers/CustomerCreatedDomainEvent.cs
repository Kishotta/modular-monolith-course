namespace Evently.Modules.Ticketing.Domain.Customers;

public sealed class CustomerCreatedDomainEvent(Guid customerId) : DomainEvent
{
    public Guid CustomerId { get; } = customerId;
}