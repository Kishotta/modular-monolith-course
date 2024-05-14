namespace Evently.Modules.Events.Domain.Categories;

public class CategoryArchivedDomainEvent(Guid categoryId) : DomainEvent
{
    public Guid CategoryId { get; init; } = categoryId;
}