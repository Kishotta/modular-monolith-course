using Evently.Common.Domain;

namespace Evently.Modules.Users.Domain.Users;

[Auditable]
public sealed class User : Entity
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    
    private User() { }
    
    public static User Create(
        string email,
        string firstName,
        string lastName)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            FirstName = firstName,
            LastName = lastName
        };
        
        user.RaiseDomainEvent(new UserRegisteredDomainEvent(user.Id));

        return user;
    }
    
    public void Update(string firstName, string lastName)
    {
        if (FirstName == firstName && LastName == lastName) return;
        
        FirstName = firstName;
        LastName = lastName;
        
        RaiseDomainEvent(new UserProfileUpdatedDomainEvent(Id, FirstName, LastName));
    }
    
}