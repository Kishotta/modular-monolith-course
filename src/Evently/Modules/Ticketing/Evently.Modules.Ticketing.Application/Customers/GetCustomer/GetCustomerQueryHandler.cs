using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Ticketing.Domain.Customers;

namespace Evently.Modules.Ticketing.Application.Customers.GetCustomer;

internal sealed class GetCustomerQueryHandler(ICustomerRepository customers)
    : IQueryHandler<GetCustomerQuery, Customer>
{
    public async Task<Result<Customer>> Handle(GetCustomerQuery query, CancellationToken cancellationToken)
    {
        var customer = await customers.GetAsync(query.CustomerId, cancellationToken);

        return customer ?? Result.Failure<Customer>(CustomerErrors.NotFound(query.CustomerId));
    }
}