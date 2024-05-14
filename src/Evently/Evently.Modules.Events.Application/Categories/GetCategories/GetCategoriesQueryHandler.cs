using Dapper;
using Evently.Common.Application.Data;

namespace Evently.Modules.Events.Application.Categories.GetCategories;

internal sealed class GetCategoriesQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetCategoriesQuery, IReadOnlyCollection<CategoryResponse>>
{
    public async Task<Result<IReadOnlyCollection<CategoryResponse>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        await using var dbConnection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                id AS {nameof(CategoryResponse.Id)},
                name AS {nameof(CategoryResponse.Name)},
                is_archived AS {nameof(CategoryResponse.IsArchived)}
             FROM events.categories
             """;
        
        var categories = (await dbConnection.QueryAsync<CategoryResponse>(sql)).AsList();

        return categories;
    }
}