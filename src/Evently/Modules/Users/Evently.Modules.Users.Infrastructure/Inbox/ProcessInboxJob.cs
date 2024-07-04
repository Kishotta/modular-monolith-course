using System.Data;
using Dapper;
using Evently.Common.Application.Clock;
using Evently.Common.Application.Data;
using Evently.Common.Application.EventBus;
using Evently.Common.Infrastructure.Inbox;
using Evently.Common.Infrastructure.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace Evently.Modules.Users.Infrastructure.Inbox;

[DisallowConcurrentExecution]
internal sealed class ProcessInboxJob(
    IDbConnectionFactory dbConnectionFactory,
    IServiceScopeFactory serviceScopeFactory,
    IDateTimeProvider dateTimeProvider,
    IOptions<InboxOptions> inboxOptions,
    ILogger<ProcessInboxJob> logger)
    : IJob
{
    private const string ModuleName = "Users";
    
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("{Module} - Beginning to process inbox messages", ModuleName);

        await using var connection = await dbConnectionFactory.OpenConnectionAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        var inboxMessages = await GetUnprocessedInboxMessagesAsync(connection, transaction);
        
        foreach (var inboxMessage in inboxMessages)
        {
            Exception? exception = null;
            try
            {
                var integrationEvent = JsonConvert.DeserializeObject<IIntegrationEvent>(inboxMessage.Content, SerializerSettings.Instance)!;

                await PublishIntegrationEvent(integrationEvent, context.CancellationToken);
            }
            catch (Exception caughtException)
            {
                logger.LogError(caughtException, "{Module} - Exception while processing inbox message {MessageId}", ModuleName, inboxMessage.Id);
                
                exception = caughtException;
            }

            await UpdateInboxMessageAsync(connection, transaction, inboxMessage, exception);
        }
        
        await transaction.CommitAsync();
        
        logger.LogInformation("{Module} - Completed processing inbox messages", ModuleName);
    }

    private async Task PublishIntegrationEvent(IIntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();

        var integrationEventHandlers = IntegrationEventHandlersFactory.GetHandlers(
            integrationEvent.GetType(),
            scope.ServiceProvider,
            Presentation.AssemblyReference.Assembly);

        foreach (var integrationEventHandler in integrationEventHandlers)
        {
            await integrationEventHandler.Handle(integrationEvent, cancellationToken);
        }
    }

    private async Task<IReadOnlyList<InboxMessageResponse>> GetUnprocessedInboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction transaction)
    {
        var sql =
            $"""
             SELECT
                id AS {nameof(InboxMessageResponse.Id)},
                content AS {nameof(InboxMessageResponse.Content)}
             FROM users.inbox_messages
             WHERE processed_at_utc IS NULL
             ORDER BY occurred_at_utc
             LIMIT {inboxOptions.Value.BatchSize}
             FOR UPDATE
             """;

        var inboxMessages = await connection.QueryAsync<InboxMessageResponse>(sql, transaction: transaction);
        return inboxMessages.ToList();
    }
    
    private async Task UpdateInboxMessageAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        InboxMessageResponse inboxMessage,
        Exception? exception)
    {
        const string sql =
            """
            UPDATE users.inbox_messages
            SET processed_at_utc = @ProcessedAtUtc,
               error = @Error
            WHERE id = @Id
            """;

        await connection.ExecuteAsync(
            sql, 
            new
            {
                inboxMessage.Id,
                ProcessedAtUtc = dateTimeProvider.UtcNow,
                Error = exception?.Message
            }, transaction: transaction);
    }

    internal sealed record InboxMessageResponse(Guid Id, string Content);
}