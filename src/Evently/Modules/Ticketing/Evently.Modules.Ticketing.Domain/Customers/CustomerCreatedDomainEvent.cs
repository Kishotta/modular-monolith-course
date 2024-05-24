namespace Evently.Modules.Ticketing.Domain.Customers;

public class CustomerCreatedDomainEvent(Guid customerId) : DomainEvent
{
    public Guid CustomerId { get; } = customerId;
}