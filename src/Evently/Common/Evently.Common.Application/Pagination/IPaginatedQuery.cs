using Evently.Common.Application.Messaging;

namespace Evently.Common.Application.Pagination;

public interface IPaginatedQuery<TResponse> : IQuery<PaginatedResponse<TResponse>>
{
    int Page     { get; }
    int PageSize { get; }
}