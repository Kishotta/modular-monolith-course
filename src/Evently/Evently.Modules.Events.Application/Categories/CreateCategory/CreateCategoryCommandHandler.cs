using Evently.Modules.Events.Domain.Categories;

namespace Evently.Modules.Events.Application.Categories.CreateCategory;

internal sealed class CreateCategoryCommandHandler(
    ICategoryRepository categories,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateCategoryCommand, CategoryResponse>
{
    public async Task<Result<CategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = Category.Create(request.Name);
        
        categories.Insert(category);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CategoryResponse(
            category.Id,
            category.Name,
            category.IsArchived);
    }
}