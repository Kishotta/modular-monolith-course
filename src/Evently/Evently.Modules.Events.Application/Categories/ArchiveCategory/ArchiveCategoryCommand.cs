namespace Evently.Modules.Events.Application.Categories.ArchiveCategory;

public record ArchiveCategoryCommand(Guid CategoryId) : ICommand<CategoryResponse>;