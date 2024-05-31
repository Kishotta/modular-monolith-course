using System.Data;
using Dapper;
using Evently.Common.Application.Clock;
using Evently.Common.Application.Data;
using Evently.Common.Domain;
using Evently.Common.Infrastructure.Outbox;
using Evently.Common.Infrastructure.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace Evently.Modules.Ticketing.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxJob(
    IDbConnectionFactory dbConnectionFactory,
    IServiceScopeFactory serviceScopeFactory,
    IDateTimeProvider dateTimeProvider,
    IOptions<OutboxOptions> outboxOptions,
    ILogger<ProcessOutboxJob> logger)
    : IJob
{
    private const string ModuleName = "Ticketing";
    
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
            Application.AssemblyReference.Assembly);

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
             FROM ticketing.outbox_messages
             WHERE processed_at_utc IS NULL
             ORDER BY occurred_at_utc
             LIMIT {outboxOptions.Value.BatchSize}
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
        const string sql =
            """
            UPDATE ticketing.outbox_messages
            SET processed_at_utc = @ProcessedAtUtc,
               error = @Error
            WHERE id = @Id
            """;

        await connection.ExecuteAsync(
            sql, 
            new
            {
                Id = outboxMessage.Id,
                ProcessedAtUtc = dateTimeProvider.UtcNow,
                Error = exception?.Message
            }, transaction: transaction);
    }

    internal sealed record OutboxMessageResponse(Guid Id, string Content);
}