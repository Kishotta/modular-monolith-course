namespace Evently.Modules.Events.Application.Categories.GetCategory;

public record GetCategoryQuery(Guid CategoryId) : IQuery<CategoryResponse>;