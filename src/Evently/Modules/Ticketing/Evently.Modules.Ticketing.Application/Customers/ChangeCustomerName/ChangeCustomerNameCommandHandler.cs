using Evently.Modules.Ticketing.Application.Abstractions.Data;
using Evently.Modules.Ticketing.Domain.Customers;

namespace Evently.Modules.Ticketing.Application.Customers.ChangeCustomerName;

internal sealed class ChangeCustomerNameCommandHandler(
    ICustomerRepository customers,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ChangeCustomerNameCommand>
{
    public async Task<Result> Handle(ChangeCustomerNameCommand request, CancellationToken cancellationToken)
    {
        var customer = await customers.GetAsync(request.CustomerId, cancellationToken);
        if (customer is null)
            return Result.Failure(CustomerErrors.NotFound(request.CustomerId));
        
        customer.ChangeName(request.FirstName, request.LastName);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}