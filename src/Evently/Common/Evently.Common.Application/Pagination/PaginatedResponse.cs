namespace Evently.Common.Application.Pagination;

public class PaginatedResponse<TResponse>
{
    public TResponse Data     { get; init; } = default!;
    public PageData  PageData { get; init; } = default!;

    public static PaginatedResponse<TResponse> Create(
        TResponse                  data,
        IPaginatedQuery<TResponse> query,
        int                        totalCount) =>
        new()
        {
            Data     = data,
            PageData = PageData.Create(query.Page, query.PageSize, totalCount)
        };
    
    public static PaginatedResponse<TResponse> Create(
        TResponse data,
        PageData  pageData) =>
        new()
        {
            Data     = data,
            PageData = pageData
        };
}