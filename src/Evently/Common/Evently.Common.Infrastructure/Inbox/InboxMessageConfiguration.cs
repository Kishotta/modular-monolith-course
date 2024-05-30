using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Common.Infrastructure.Inbox;

public sealed class InboxMessageConfiguration : IEntityTypeConfiguration<InboxMessage>
{
    public void Configure(EntityTypeBuilder<InboxMessage> builder)
    {
        builder.ToTable("inbox_messages");
        
        builder.HasKey(outboxMessage => outboxMessage.Id);

        builder.Property(outBoxMessage => outBoxMessage.Content)
            .HasMaxLength(3000)
            .HasColumnType("jsonb");
    }
}