using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Ticketing.Application.Abstractions.Data;
using Evently.Modules.Ticketing.Domain.Customers;

namespace Evently.Modules.Ticketing.Application.Customers.CreateCustomer;

internal sealed class CreateCustomerCommandHandler(
    ICustomerRepository customers,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateCustomerCommand>
{
    public async Task<Result> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = Customer.Create(request.CustomerId, request.Email, request.FirstName, request.LastName);
        
        customers.Insert(customer);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}