using Microsoft.Extensions.Options;
using Quartz;

namespace Evently.Modules.Attendance.Infrastructure.Inbox;

internal sealed class ConfigureProcessInboxJob(IOptions<InboxOptions> inboxOptions)
    : IConfigureOptions<QuartzOptions>
{
    private readonly InboxOptions _inboxOptions = inboxOptions.Value;
    
    public void Configure(QuartzOptions options)
    {
        var jobName = typeof(ProcessInboxJob).FullName!;

        options.AddJob<ProcessInboxJob>(configure => configure.WithIdentity(jobName))
            .AddTrigger(configure =>
                configure
                    .ForJob(jobName)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithIntervalInSeconds(_inboxOptions.IntervalInSeconds).RepeatForever()));
    }
}