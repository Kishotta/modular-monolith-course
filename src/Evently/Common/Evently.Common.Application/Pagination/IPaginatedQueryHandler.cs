using Evently.Common.Domain;
using MediatR;

namespace Evently.Common.Application.Pagination;

public interface IPaginatedQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<PaginatedResponse<TResponse>>>
    where TQuery : IPaginatedQuery<TResponse>, IRequest<Result<PaginatedResponse<TResponse>>>;