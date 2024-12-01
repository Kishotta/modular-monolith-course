using Evently.Modules.Ticketing.Application.Abstractions.Data;
using Evently.Modules.Ticketing.Application.Abstractions.Payments;
using Evently.Modules.Ticketing.Application.Carts;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Domain.Payments;

namespace Evently.Modules.Ticketing.Application.Orders.CreateOrder;

internal sealed class CreateOrderCommandHandler(
    ICustomerRepository customers,
    CartService cartService,
    ITicketTypeRepository ticketTypes,
    IOrderRepository orders,
    IPaymentService paymentService,
    IPaymentRepository payments,
    IUnitOfWork unitOfWork
) 
    : ICommandHandler<CreateOrderCommand>
{
    public async Task<Result> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        
        var customer = await customers.GetAsync(request.CustomerId, cancellationToken);
        if (customer is null)
            return CustomerErrors.NotFound(request.CustomerId);

        var order = Order.Create(customer);

        var cart = await cartService.GetAsync(customer.Id, cancellationToken);
        if (cart.Items.Count == 0)
            return CartErrors.Empty;

        var result = await AddCartItemsToOrder(cart, order, cancellationToken);
        if (result.IsFailure)
            return result.Error;

        var paymentResponse = await paymentService.ChargeAsync(order.TotalPrice, order.Currency);

        var payment = Payment.Create(
            order,
            paymentResponse.TransactionId,
            paymentResponse.Amount,
            paymentResponse.Currency);

        payments.Insert(payment);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        await cartService.ClearAsync(customer.Id, cancellationToken);

        return Result.Success();
    }

    private async Task<Result> AddCartItemsToOrder(Cart cart, Order order, CancellationToken cancellationToken)
    {
        foreach (var cartItem in cart.Items)
        {
            var ticketType = await ticketTypes.GetWithLockAsync(cartItem.TicketTypeId, cancellationToken);
            if (ticketType is null)
                return TicketTypeErrors.NotFound(cartItem.TicketTypeId);

            var result = ticketType.UpdateQuantity(cartItem.Quantity);
            if (result.IsFailure)
                return result.Error;
            
            order.AddItem(ticketType, cartItem.Quantity, cartItem.Price, cartItem.Currency);
        }
        
        orders.Insert(order);

        return Result.Success();
    }
}