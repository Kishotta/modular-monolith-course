namespace Evently.Modules.Ticketing.Domain.Customers;

public sealed class Customer : Entity
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    
    private Customer() { }

    public static Customer Create(Guid id, string email, string firstName, string lastName) =>
        new()
        {
            Id = id,
            Email = email,
            FirstName = firstName,
            LastName = lastName
        };

    public void Update(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}
