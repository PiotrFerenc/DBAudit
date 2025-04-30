using DBAudit.Infrastructure.DatabaseProvider.Migrations;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DBAudit.Infrastructure.DatabaseProvider.SqlServer.Extensions;

public static class SqlServerExtensions
{
    public static void AddSqlServerProvider<TDbContext>(this IServiceCollection services, string connectionString) where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>(options => options.UseSqlServer(connectionString));

        services.AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(DatabaseMigration).Assembly).For.Migrations()
            )
            .AddLogging(logging => logging.AddFluentMigratorConsole());
    }
}