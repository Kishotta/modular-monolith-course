using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Ticketing.Domain.Customers;

namespace Evently.Modules.Ticketing.Application.Carts.ClearCart;

internal sealed class ClearCartCommandHandler (
    ICustomerRepository customers,
    CartService cartService): ICommandHandler<ClearCartCommand>
{
    public async Task<Result> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var customer = await customers.GetAsync(request.CustomerId, cancellationToken);

        if (customer is null)
            return Result.Failure(CustomerErrors.NotFound(request.CustomerId));

        await cartService.ClearAsync(customer.Id, cancellationToken);

        return Result.Success();
    }
}