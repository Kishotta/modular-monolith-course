using Evently.Modules.Events.Domain.Categories;

namespace Evently.Modules.Events.Application.Categories.UpdateCategory;

internal sealed class UpdateCategoryCommandHandler(
    ICategoryRepository categories,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateCategoryCommand, CategoryResponse>
{
    public async Task<Result<CategoryResponse>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categories.GetAsync(request.CategoryId, cancellationToken);
        if (category is null)
            return CategoryErrors.NotFound(request.CategoryId);

        category.ChangeName(request.Name);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return (CategoryResponse)category;
    }
}