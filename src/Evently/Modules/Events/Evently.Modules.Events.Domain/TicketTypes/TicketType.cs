using Evently.Common.Domain.Auditing;
using Evently.Modules.Events.Domain.Events;

namespace Evently.Modules.Events.Domain.TicketTypes;

[Auditable]
public sealed class TicketType : Entity
{
    public Guid Id { get; private set; }
    public Guid EventId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public string Currency { get; private set; } = string.Empty;
    public decimal Quantity { get; private set; }
    
    private TicketType() { }

    public static TicketType Create(
        Event @event,
        string name,
        decimal price,
        string currency,
        decimal quantity)
    {
        var ticketType = new TicketType
        {
            Id = Guid.NewGuid(),
            EventId = @event.Id,
            Name = name,
            Price = price,
            Currency = currency,
            Quantity = quantity
        };

        ticketType.RaiseDomainEvent(new TicketTypeCreatedDomainEvent(ticketType.Id));

        return ticketType;
    }

    public void UpdatePrice(decimal price)
    {
        if (Price == price) return;

        Price = price;
        
        RaiseDomainEvent(new TicketTypePriceChangedDomainEvent(Id, Price));
    }
}