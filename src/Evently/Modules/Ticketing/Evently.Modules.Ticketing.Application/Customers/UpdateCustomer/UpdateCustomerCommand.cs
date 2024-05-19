using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Ticketing.Application.Abstractions.Data;
using Evently.Modules.Ticketing.Domain.Customers;
using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Customers.UpdateCustomer;

public record UpdateCustomerCommand(Guid CustomerId, string FirstName, string LastName) : ICommand;

internal sealed class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(command => command.CustomerId).NotEmpty();
        RuleFor(command => command.FirstName).NotEmpty();
        RuleFor(command => command.LastName).NotEmpty();
    }
}

internal sealed class UpdateCustomerCommandHandler(
    ICustomerRepository customers,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateCustomerCommand>
{
    public async Task<Result> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await customers.GetAsync(request.CustomerId, cancellationToken);
        if (customer is null)
            return Result.Failure(CustomerErrors.NotFound(request.CustomerId));
        
        customer.Update(request.FirstName, request.LastName);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}