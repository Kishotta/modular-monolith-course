using Evently.Common.Domain.Auditing;

namespace Evently.Modules.Ticketing.Domain.Customers;

[Auditable]
public sealed class Customer : Entity
{
    public Guid Id { get; private init; }
    public string Email { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    
    private Customer() { }

    public static Customer Create(Guid id, string email, string firstName, string lastName)
    {
        var customer = new Customer
        {
            Id = id,
            Email = email,
            FirstName = firstName,
            LastName = lastName
        };
        
        customer.RaiseDomainEvent(new CustomerCreatedDomainEvent(customer.Id));

        return customer;
    }

    public void ChangeName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        
        RaiseDomainEvent(new CustomerNameChangedDomainEvent(Id, FirstName, LastName));
    }
}