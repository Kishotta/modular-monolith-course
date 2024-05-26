namespace Evently.Modules.Ticketing.Domain.Customers;

public sealed class CustomerNameChangedDomainEvent(
    Guid customerId, 
    string firstName,
    string lastName) : DomainEvent
{
    public Guid CustomerId { get; } = customerId;
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;
}