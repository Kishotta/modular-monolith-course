namespace Evently.Common.Application.Pagination;

public abstract record PaginatedQuery<TResponse>(int Page, int PageSize) : IPaginatedQuery<TResponse>
{
    public int Limit  => PageSize;
    public int Offset => (Page - 1) * PageSize;

    public object PaginationFilter => new { Limit, Offset };
}