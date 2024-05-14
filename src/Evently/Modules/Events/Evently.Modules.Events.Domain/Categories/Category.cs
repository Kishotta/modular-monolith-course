namespace Evently.Modules.Events.Domain.Categories;

public class Category : Entity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public bool IsArchived { get; private set; }
    
    private Category() {}

    public static Category Create(string name)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = name,
            IsArchived = false
        };
        
        category.RaiseDomainEvent(new CategoryCreatedDomainEvent(category.Id));

        return category;
    }

    public void Archive()
    {
        IsArchived = true;
        
        RaiseDomainEvent(new CategoryArchivedDomainEvent(Id));
    }

    public void ChangeName(string name)
    {
        if (Name == name) return;

        Name = name;
        
        RaiseDomainEvent(new CategoryNameChangedDomainEvent(Id, name));
    }
}