namespace Evently.Modules.Ticketing.Application.Orders.GetOrders;

public sealed record GetOrdersQuery(Guid CustomerId) : IQuery<IReadOnlyCollection<OrderResponse>>;