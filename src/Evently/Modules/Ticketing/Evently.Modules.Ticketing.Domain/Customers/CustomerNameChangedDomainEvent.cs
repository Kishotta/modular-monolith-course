namespace Evently.Modules.Ticketing.Domain.Customers;

public class CustomerNameChangedDomainEvent(
    Guid customerId, 
    string firstName,
    string lastName) : DomainEvent
{
    public Guid CustomerId { get; } = customerId;
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;
}