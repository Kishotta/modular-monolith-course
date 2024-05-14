using Dapper;
using Evently.Modules.Events.Domain.Categories;

namespace Evently.Modules.Events.Application.Categories.GetCategory;

public class GetCategoryQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetCategoryQuery, CategoryResponse>
{
    public async Task<Result<CategoryResponse>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        await using var dbConnection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                id AS {nameof(CategoryResponse.Id)},
                name AS {nameof(CategoryResponse.Name)},
                is_archived AS {nameof(CategoryResponse.IsArchived)}
             FROM events.categories
             WHERE id = @CategoryId
             """;

        var category = await dbConnection.QuerySingleOrDefaultAsync<CategoryResponse>(sql, request);

        if (category is null)
            return Result.Failure<CategoryResponse>(CategoryErrors.NotFound(request.CategoryId));

        return category;
    }
}