using Evently.Common.Application.EventBus;

namespace Evently.Modules.Ticketing.IntegrationEvents;

public sealed class OrderCreatedIntegrationEvent(
    Guid id,
    DateTime occuredOnUtc,
    Guid orderId,
    Guid customerId,
    decimal totalPrice,
    DateTime createdAtUtc,
    List<OrderItemModel> orderItems)
    : IntegrationEvent(id, occuredOnUtc)
{
    public Guid OrderId { get; init; } = orderId;
    public Guid CustomerId { get; init; } = customerId;
    public decimal TotalPrice { get; init; } = totalPrice;
    public DateTime CreatedAtUtc { get; init; } = createdAtUtc;
    public List<OrderItemModel> OrderItems { get; init; } = orderItems;
}