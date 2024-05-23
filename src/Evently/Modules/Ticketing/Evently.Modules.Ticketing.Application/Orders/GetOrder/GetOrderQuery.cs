using System.Data.Common;

namespace Evently.Modules.Ticketing.Application.Orders.GetOrder;

public record GetOrderQuery(Guid OrderId) : IQuery<OrderResponse>;