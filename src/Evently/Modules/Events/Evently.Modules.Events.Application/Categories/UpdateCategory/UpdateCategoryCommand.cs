namespace Evently.Modules.Events.Application.Categories.UpdateCategory;

public record UpdateCategoryCommand(Guid CategoryId, string Name) : ICommand<CategoryResponse>;