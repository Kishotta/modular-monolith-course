using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Common.Infrastructure.Outbox;

namespace Evently.Modules.Users.Infrastructure.Outbox;

public class IdempotentDomainEventHandler<TDomainEvent>(
    IDomainEventHandler<TDomainEvent> decorated,
    IDbConnectionFactory dbConnectionFactory) 
    : DomainEventHandler<TDomainEvent> 
    where TDomainEvent : IDomainEvent
{
    public override async Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        await using var connection = await dbConnectionFactory.OpenConnectionAsync();

        var outboxMessageConsumer = new OutboxMessageConsumer(domainEvent.Id, decorated.GetType().Name);

        if (await OutboxConsumerExistsAsync(connection, outboxMessageConsumer)) return;
        
        await decorated.Handle(domainEvent, cancellationToken);

        await InsertOutboxConsumerAsync(connection, outboxMessageConsumer);
    }

    private async Task<bool> OutboxConsumerExistsAsync(
        DbConnection connection, 
        OutboxMessageConsumer outboxMessageConsumer)
    {
        const string sql =
            """
            SELECT EXISTS(
                SELECT 1
                FROM users.outbox_message_consumers
                WHERE outbox_message_id = @OutboxMessageId AND 
                      name = @Name
            )
            """;
        
        return await connection.ExecuteScalarAsync<bool>(sql, outboxMessageConsumer);
    }

    private async Task InsertOutboxConsumerAsync(DbConnection connection, OutboxMessageConsumer outboxMessageConsumer)
    {
        const string sql =
            """
            INSERT INTO users.outbox_message_consumers(outbox_message_id, name)
            VALUES (@OutboxMessageId, @Name)
            """;
        
        await connection.ExecuteAsync(sql, outboxMessageConsumer);
    }
}