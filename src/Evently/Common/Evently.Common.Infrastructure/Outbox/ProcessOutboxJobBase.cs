using System.Data;
using System.Diagnostics;
using System.Reflection;
using Dapper;
using Evently.Common.Application.Clock;
using Evently.Common.Application.Data;
using Evently.Common.Domain;
using Evently.Common.Infrastructure.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;

namespace Evently.Common.Infrastructure.Outbox;

public abstract class ProcessOutboxJobBase(
    IDbConnectionFactory dbConnectionFactory,
    IServiceScopeFactory serviceScopeFactory,
    IDateTimeProvider dateTimeProvider,
    ILogger logger) : IJob
{
    protected abstract string ModuleName { get; }
    protected abstract Assembly ApplicationAssembly { get; }
    protected abstract string Schema { get; }
    protected abstract int BatchSize { get; }
    
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("{Module} - Beginning to process outbox messages", ModuleName);
        
        await using var connection = await dbConnectionFactory.OpenConnectionAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        var outboxMessages = await GetUnprocessedOutboxMessagesAsync(connection, transaction);
        
        foreach (var outboxMessage in outboxMessages)
        {
            Exception? exception = null;
            try
            {
                var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content, SerializerSettings.Instance)!;

                await PublishDomainEvent(domainEvent);
            }
            catch (Exception caughtException)
            {
                logger.LogError(caughtException, "{Module} - Exception while processing outbox message {MessageId}", ModuleName, outboxMessage.Id);
                
                exception = caughtException;
            }
            
            await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception);
        }
        
        await transaction.CommitAsync();
        
        logger.LogInformation("{Module} - Completed processing outbox messages", ModuleName);
    }

    private async Task PublishDomainEvent(IDomainEvent domainEvent)
    {
        using var scope = serviceScopeFactory.CreateScope();

        var domainEventHandlers = DomainEventHandlersFactory.GetHandlers(
            domainEvent.GetType(),
            scope.ServiceProvider,
            ApplicationAssembly);

        foreach (var domainEventHandler in domainEventHandlers)
        {
            await domainEventHandler.Handle(domainEvent);
        }
    }

    private async Task<IReadOnlyList<OutboxMessageResponse>> GetUnprocessedOutboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction transaction)
    {
        var sql =
            $"""
             SELECT
                id AS {nameof(OutboxMessageResponse.Id)},
                content AS {nameof(OutboxMessageResponse.Content)}
             FROM {Schema}.outbox_messages
             WHERE processed_at_utc IS NULL
             ORDER BY occurred_at_utc
             LIMIT {BatchSize}
             FOR UPDATE
             """;

        var outboxMessages = await connection.QueryAsync<OutboxMessageResponse>(sql, transaction: transaction);
        return outboxMessages.ToList();
    }
    
    private async Task UpdateOutboxMessageAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        OutboxMessageResponse outboxMessage,
        Exception? exception)
    {
        var sql =
            $"""
             UPDATE {Schema}.outbox_messages
             SET processed_at_utc = @ProcessedAtUtc,
                error = @Error
             WHERE id = @Id
             """;

        await connection.ExecuteAsync(
            sql, 
            new
            {
                outboxMessage.Id,
                ProcessedAtUtc = dateTimeProvider.UtcNow,
                Error = exception?.Message
            }, transaction: transaction);
    }

    private sealed record OutboxMessageResponse(Guid Id, string Content);
}