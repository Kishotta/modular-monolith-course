namespace Evently.Modules.Ticketing.Application.Orders.GetOrders;

public record GetOrdersQuery(Guid CustomerId) : IQuery<IReadOnlyCollection<OrderResponse>>;