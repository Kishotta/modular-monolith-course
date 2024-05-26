using Evently.Modules.Events.Domain.TicketTypes;

namespace Evently.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;

internal sealed class UpdateTicketTypePriceCommandHandler(
    ITicketTypeRepository ticketTypes,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateTicketTypePriceCommand, TicketTypeResponse>
{
    public async Task<Result<TicketTypeResponse>> Handle(UpdateTicketTypePriceCommand request, CancellationToken cancellationToken)
    {
        var ticketType = await ticketTypes.GetAsync(request.TicketTypeId, cancellationToken);

        if (ticketType is null)
            return Result.Failure<TicketTypeResponse>(TicketTypeErrors.NotFound(request.TicketTypeId));
        
        ticketType.UpdatePrice(request.Price);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return (TicketTypeResponse)ticketType;
    }
}