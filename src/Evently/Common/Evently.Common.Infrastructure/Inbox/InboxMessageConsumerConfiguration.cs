using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Common.Infrastructure.Inbox;

public sealed class InboxMessageConsumerConfiguration : IEntityTypeConfiguration<InboxMessageConsumer>
{
    public void Configure(EntityTypeBuilder<InboxMessageConsumer> builder)
    {
        builder.ToTable("inbox_message_consumers");
        
        builder.HasKey(inboxMessageConsumer => new { inboxMessageConsumer.InboxMessageId, inboxMessageConsumer.Name });

        builder.Property(inboxMessageConsumer => inboxMessageConsumer.Name).HasMaxLength(500);
    }
}