using MediatR;

namespace Evently.Common.Domain;

public interface IDomainEvent : INotification
{
    Guid Id { get; }
    DateTime OccuredAtUtc { get; }
}