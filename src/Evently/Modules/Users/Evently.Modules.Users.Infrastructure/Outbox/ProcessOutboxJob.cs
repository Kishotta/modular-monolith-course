using System.Reflection;
using Evently.Common.Application.Clock;
using Evently.Common.Application.Data;
using Evently.Common.Infrastructure.Database;
using Evently.Common.Infrastructure.Outbox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace Evently.Modules.Users.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxJob(
    IDbConnectionFactory dbConnectionFactory,
    IServiceScopeFactory serviceScopeFactory,
    IDateTimeProvider dateTimeProvider,
    IOptions<OutboxOptions> outboxOptions,
    ILogger<ProcessOutboxJob> logger)
    : ProcessOutboxJobBase(dbConnectionFactory, serviceScopeFactory, dateTimeProvider, logger)
{
    protected override string ModuleName => "Users";
    protected override Assembly ApplicationAssembly => Application.AssemblyReference.Assembly;
    protected override string Schema => Schemas.Users;
    protected override int BatchSize => outboxOptions.Value.BatchSize;
}