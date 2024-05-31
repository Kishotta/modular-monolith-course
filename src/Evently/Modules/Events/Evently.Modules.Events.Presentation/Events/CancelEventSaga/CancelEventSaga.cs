using Evently.Modules.Events.IntegrationEvents;
using Evently.Modules.Ticketing.IntegrationEvents;
using MassTransit;

namespace Evently.Modules.Events.Presentation.Events.CancelEventSaga;

public sealed class CancelEventSaga : MassTransitStateMachine<CancelEventState>
{
    public State CancellationStarted { get; private set; } = default!;
    public State PaymentsRefunded { get; private set; } = default!;
    public State TicketsArchived { get; private set; } = default!;
   
    public Event<EventCancelledIntegrationEvent> EventCancelled { get; private set; } = default!;
    public Event<EventPaymentsRefundedIntegrationEvent> EventPaymentsRefunded { get; private set; } = default!;
    public Event<EventTicketsArchivedIntegrationEvent> EventTicketsArchived { get; private set; } = default!;
    public Event EventCancellationCompleted { get; private set; } = default!;

    public CancelEventSaga()
    {
        Event(() => EventCancelled, configurator => configurator.CorrelateById(context => context.Message.EventId));
        Event(() => EventPaymentsRefunded, configurator => configurator.CorrelateById(context => context.Message.EventId));
        Event(() => EventTicketsArchived, configurator => configurator.CorrelateById(context => context.Message.EventId));
        
        InstanceState(state => state.CurrentState);

        Initially(
            When(EventCancelled)
                .Publish(context => new EventCancellationStartedIntegrationEvent(
                    context.Message.Id,
                    context.Message.OccurredAtUtc,
                    context.Message.EventId))
                .TransitionTo(CancellationStarted));
        
        During(CancellationStarted,
            When(EventPaymentsRefunded)
                .TransitionTo(PaymentsRefunded),
            When(EventTicketsArchived)
                .TransitionTo(TicketsArchived));
        
        During(PaymentsRefunded,
            When(EventTicketsArchived)
                .TransitionTo(TicketsArchived));

        During(TicketsArchived,
            When(EventPaymentsRefunded)
                .TransitionTo(PaymentsRefunded));

        CompositeEvent(() => EventCancellationCompleted,
            state => state.CancellationCompletedStatus,
            EventPaymentsRefunded, EventTicketsArchived);
        
        DuringAny(
            When(EventCancellationCompleted)
                .Publish(context => new EventCancellationCompletedIntegrationEvent(
                    Guid.NewGuid(),
                    DateTime.UtcNow,
                    context.Saga.CorrelationId))
                .Finalize());
    }
}