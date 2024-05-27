using Evently.Modules.Attendance.Domain.Attendees;
using Evently.Modules.Attendance.Domain.Events;
using Evently.Modules.Attendance.Domain.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Modules.Attendance.Infrastructure.Tickets;

internal sealed class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasKey(ticket => ticket.Id);

        builder.Property(ticket => ticket.Code).HasMaxLength(30);

        builder.HasIndex(ticket => ticket.Code).IsUnique();

        builder.HasOne<Attendee>().WithMany().HasForeignKey(ticket => ticket.AttendeeId);

        builder.HasOne<Event>().WithMany().HasForeignKey(ticket => ticket.EventId);
    }
}