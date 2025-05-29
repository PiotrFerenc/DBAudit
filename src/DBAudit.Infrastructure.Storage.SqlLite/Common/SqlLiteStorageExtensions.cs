using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.DatabaseProvider;
using DBAudit.Infrastructure.Storage.Metrics;
using DBAudit.Infrastructure.Storage.SqlLite.Metrics;
using FluentMigrator.Runner;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DBAudit.Infrastructure.Storage.SqlLite.Common;

public static class SqlLiteStorageExtensions
{
    public static void AddAddSqlLiteStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("SqlLite");
        services.AddDbContext<SqlLiteDbContext>(x => x.UseSqlite(cs));
        services.AddScoped<IEnvironmentService, SqlLiteEnvironmentService>();
        services.AddScoped<IDatabaseService, SqlLiteDatabaseService>();
        services.AddScoped<ITableService, SqlLiteTableService>();
        services.AddScoped<IColumnService, SqlLiteColumnService>();

        services.AddTransient<IMetricsService, ColumnMetricsService>();
        
            services
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddSQLite()
                .WithGlobalConnectionString(cs)
                .ScanIn(typeof(IDatabaseProvider).Assembly).For.All())
            .AddLogging(lb => lb.AddFluentMigratorConsole());
    }
}
