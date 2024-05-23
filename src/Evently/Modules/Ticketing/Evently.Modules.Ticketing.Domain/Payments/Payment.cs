using Evently.Common.Domain;
using Evently.Modules.Ticketing.Domain.Orders;

namespace Evently.Modules.Ticketing.Domain.Payments;

public sealed class Payment : Entity
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid TransactionId { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = string.Empty;
    public decimal? AmountRefunded { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? RefundedAtUtc { get; private set; } = null;
    
    private Payment() { }

    public static Payment Create(Order order, Guid transationId, decimal amount, string currency)
    {
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            TransactionId = transationId,
            Amount = amount,
            Currency = currency,
            CreatedAtUtc = DateTime.UtcNow
        };

        payment.RaiseDomainEvent(new PaymentCreatedDomainEvent(payment.Id));

        return payment;
    }

    public Result Refund(decimal refundAmount)
    {
        if (AmountRefunded == Amount)
            return Result.Failure(PaymentErrors.AlreadyRefunded);

        if (AmountRefunded + refundAmount > Amount)
            return Result.Failure(PaymentErrors.NotEnoughFunds);
        
        AmountRefunded += refundAmount;
        
        if (Amount == AmountRefunded)
            RaiseDomainEvent(new PaymentRefundedDomainEvent(Id, TransactionId, refundAmount));
        else
            RaiseDomainEvent(new PaymentPartiallyRefundedDomainEvent(Id, TransactionId, refundAmount));

        return Result.Success();
    }
}