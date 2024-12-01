using Evently.Common.Domain.Auditing;

namespace Evently.Modules.Ticketing.Domain.Events;

[Auditable]
public sealed class TicketType : Entity
{
    public Guid Id { get; private set; }
    public Guid EventId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public string Currency { get; private set; } = string.Empty;
    public decimal Quantity { get; private set; }
    public decimal AvailableQuantity { get; private set; }

    private TicketType() {}
    
    public static TicketType Create(
        Guid id,
        Guid eventId,
        string name,
        decimal price,
        string currency,
        decimal quantity)
    {
        var ticketType = new TicketType
        {
            Id = id,
            EventId = eventId,
            Name = name,
            Price = price,
            Currency = currency,
            Quantity = quantity,
            AvailableQuantity = quantity
        };

        return ticketType;
    }

    public void UpdatePrice(decimal price)
    {
        Price = price;
    }

    public Result UpdateQuantity(decimal quantity)
    {
        if (AvailableQuantity < quantity)
            return TicketTypeErrors.NotEnoughQuantity(AvailableQuantity);

        AvailableQuantity -= quantity;
        
        if (AvailableQuantity == 0)
            RaiseDomainEvent(new TicketTypeSoldOutDomainEvent(Id));
        
        return Result.Success();
    }
}