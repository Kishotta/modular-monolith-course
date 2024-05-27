using Evently.Modules.Attendance.Domain.Attendees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Modules.Attendance.Infrastructure.Attendees;

internal sealed class AttendeeConfiguration : IEntityTypeConfiguration<Attendee>
{
    public void Configure(EntityTypeBuilder<Attendee> builder)
    {
        builder.HasKey(attendee => attendee.Id);

        builder.Property(attendee => attendee.FirstName).HasMaxLength(200);

        builder.Property(attendee => attendee.LastName).HasMaxLength(200);

        builder.Property(attendee => attendee.Email).HasMaxLength(300);
    }
}