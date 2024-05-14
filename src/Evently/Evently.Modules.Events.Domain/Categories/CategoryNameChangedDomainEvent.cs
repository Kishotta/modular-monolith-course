namespace Evently.Modules.Events.Domain.Categories;

public class CategoryNameChangedDomainEvent(Guid categoryId, string name) : DomainEvent
{
    public Guid CategoryId { get; init; } = categoryId;
    public string Name { get; init;  } = name;
}