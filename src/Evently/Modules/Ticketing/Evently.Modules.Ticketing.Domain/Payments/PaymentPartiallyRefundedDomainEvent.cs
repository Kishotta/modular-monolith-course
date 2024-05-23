using Evently.Common.Domain;

namespace Evently.Modules.Ticketing.Domain.Payments;

public sealed class PaymentPartiallyRefundedDomainEvent(Guid paymentId, Guid transactionId, decimal refundAmount) : DomainEvent
{
    public Guid PaymentId { get; } = paymentId;
    public Guid TransactionId { get; } = transactionId;
    public decimal RefundAmount { get; } = refundAmount;
}