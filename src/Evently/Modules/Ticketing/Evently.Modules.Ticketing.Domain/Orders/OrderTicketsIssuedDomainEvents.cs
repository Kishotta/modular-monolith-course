using Evently.Common.Domain;

namespace Evently.Modules.Ticketing.Domain.Orders;

public class OrderTicketsIssuedDomainEvents(Guid orderId) : DomainEvent
{
    public Guid OrderId { get; } = orderId;
}