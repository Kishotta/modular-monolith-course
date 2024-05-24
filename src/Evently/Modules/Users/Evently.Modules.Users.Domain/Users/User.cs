using Evently.Common.Domain;
using Evently.Common.Domain.Auditing;

namespace Evently.Modules.Users.Domain.Users;

[Auditable]
public sealed class User : Entity
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string IdentityId { get; private set; } = string.Empty;
    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();
    
    private readonly List<Role> _roles = [];
    
    private User() { }
    
    public static User Create(
        string email,
        string firstName,
        string lastName,
        string identityId)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            IdentityId = identityId
        };
        
        user._roles.Add(Role.Member);
        
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