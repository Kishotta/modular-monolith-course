namespace Evently.Common.Application.Pagination;

public sealed record PageData
{
    private PageData() {}

    public int  Page         { get; init; }
    public int  PageSize     { get; init; }
    public Uri? FirstPage    { get; init; }
    public Uri? LastPage     { get; init; }
    public int  TotalPages   { get; init; }
    public Uri? NextPage     { get; init; }
    public Uri? PreviousPage { get; init; }
    public int  TotalCount   { get; init; }

    public static PageData Create(
        int page,
        int pageSize,
        int totalCount)
    {
        var totalPages = Convert.ToInt32(Math.Ceiling((double)totalCount / pageSize));

        return new PageData
        {
            Page       = page,
            PageSize   = pageSize,
            TotalPages = totalPages,
            TotalCount = totalCount
        };
    }
};