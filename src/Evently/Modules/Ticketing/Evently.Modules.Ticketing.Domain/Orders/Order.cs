using Evently.Common.Domain.Auditing;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Events;

namespace Evently.Modules.Ticketing.Domain.Orders;

[Auditable]
public sealed class Order : Entity
{
    public Guid Id { get; private init; }
    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal TotalPrice { get; private set; }
    public string Currency { get; private set; } = string.Empty;
    public bool TicketsIssued { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    private readonly List<OrderItem> _orderItems = [];
    
    private Order() { }

    public static Order Create(Customer customer)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customer.Id,
            Status = OrderStatus.Pending,
            CreatedAtUtc = DateTime.UtcNow
        };
        
        order.RaiseDomainEvent(new OrderCreatedDomainEvent(order.Id));

        return order;
    }

    public void AddItem(TicketType ticketType, decimal quantity, decimal price, string currency)
    {
        var orderItem = OrderItem.Create(Id, ticketType.Id, quantity, price, currency);
        
        _orderItems.Add(orderItem);

        TotalPrice = _orderItems.Sum(o => o.Price);
        Currency = currency;
    }

    public Result IssueTickets()
    {
        if (TicketsIssued)
            return Result.Failure(OrderErrors.TicketsAlreadyIssued);

        TicketsIssued = true;
        
        RaiseDomainEvent(new OrderTicketsIssuedDomainEvent(Id));

        return Result.Success();
    }
}