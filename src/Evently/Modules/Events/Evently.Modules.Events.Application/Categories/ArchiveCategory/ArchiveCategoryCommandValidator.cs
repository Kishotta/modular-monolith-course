using FluentValidation;

namespace Evently.Modules.Events.Application.Categories.ArchiveCategory;

public class ArchiveCategoryCommandValidator : AbstractValidator<ArchiveCategoryCommand>
{
    public ArchiveCategoryCommandValidator()
    {
        RuleFor(command => command.CategoryId).NotEmpty();
    }
}