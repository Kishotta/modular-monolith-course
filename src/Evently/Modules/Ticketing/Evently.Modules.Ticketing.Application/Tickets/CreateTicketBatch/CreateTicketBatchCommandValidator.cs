using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Tickets.CreateTicketBatch;

internal sealed class CreateTicketBatchCommandValidator : AbstractValidator<CreateTicketBatchCommand>
{
    public CreateTicketBatchCommandValidator()
    {
        RuleFor(command => command.OrderId).NotEmpty();
    }
}