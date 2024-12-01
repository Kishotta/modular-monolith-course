using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketTypes;

namespace Evently.Modules.Events.Application.TicketTypes.CreateTicketType;

internal sealed class CreateTicketTypeCommandHandler(
    IEventRepository events,
    ITicketTypeRepository ticketTypes,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateTicketTypeCommand, TicketTypeResponse>
{
    public async Task<Result<TicketTypeResponse>> Handle(CreateTicketTypeCommand request, CancellationToken cancellationToken)
    {
        var @event = await events.GetAsync(request.EventId, cancellationToken);
        if (@event is null)
            return EventErrors.NotFound(request.EventId);

        var ticketType = TicketType.Create(
            @event,
            request.Name,
            request.Price,
            request.Currency,
            request.Quantity);
        
        ticketTypes.Insert(ticketType);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return (TicketTypeResponse)ticketType;
    }
}