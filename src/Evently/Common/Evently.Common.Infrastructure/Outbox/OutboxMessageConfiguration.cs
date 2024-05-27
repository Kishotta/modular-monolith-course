using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Common.Infrastructure.Outbox;

public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages");
        
        builder.HasKey(outboxMessage => outboxMessage.Id);

        builder.Property(outBoxMessage => outBoxMessage.Content)
            .HasMaxLength(3000)
            .HasColumnType("jsonb");
    }
}