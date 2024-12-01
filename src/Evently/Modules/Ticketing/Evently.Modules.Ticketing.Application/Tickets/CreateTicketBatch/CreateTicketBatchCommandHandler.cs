using Evently.Modules.Ticketing.Application.Abstractions.Data;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Domain.Tickets;

namespace Evently.Modules.Ticketing.Application.Tickets.CreateTicketBatch;

internal sealed class CreateTicketBatchCommandHandler(
    IOrderRepository orderRepository,
    ITicketTypeRepository ticketTypeRepository,
    ITicketRepository ticketRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateTicketBatchCommand>
{
    public async Task<Result> Handle(CreateTicketBatchCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(request.OrderId, cancellationToken);
        if (order is null)
            return OrderErrors.NotFound(request.OrderId);

        var result = order.IssueTickets();
        if (result.IsFailure)
            return result.Error;
        
        List<Ticket> tickets = [];
        foreach (var orderItem in order.OrderItems)
        {
            var ticketType = await ticketTypeRepository.GetAsync(orderItem.TicketTypeId, cancellationToken);
            if (ticketType is null)
                return TicketTypeErrors.NotFound(orderItem.TicketTypeId);

            for (var i = 0; i < orderItem.Quantity; i++)
            {
                var ticket = Ticket.Create(order, ticketType);

                tickets.Add(ticket);
            }
        }

        ticketRepository.InsertRange(tickets);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}