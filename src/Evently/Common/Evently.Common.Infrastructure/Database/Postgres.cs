using Evently.Common.Infrastructure.Auditing;
using Evently.Common.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Common.Infrastructure.Database;

public static class Postgres
{
    public static Action<IServiceProvider, DbContextOptionsBuilder> StandardOptions(IConfiguration configuration, string schema) =>
        (serviceProvider, options) =>
        {
            options.UseNpgsql(
                    configuration.GetConnectionString("Database")!,
                    optionsBuilder =>
                    {
                        optionsBuilder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, schema);
                    }).UseSnakeCaseNamingConvention()
                .AddInterceptors(
                    serviceProvider.GetRequiredService<PublishDomainEventsInterceptor>(),
                    serviceProvider.GetRequiredService<WriteAuditLogInterceptor>());
        };
}