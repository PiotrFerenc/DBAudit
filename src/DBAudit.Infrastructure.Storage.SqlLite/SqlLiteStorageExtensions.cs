using DBAudit.Infrastructure.Storage.Metrics;
using DBAudit.Infrastructure.Storage.SqlLite.Metrics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DBAudit.Infrastructure.Storage.SqlLite;

public static class SqlLiteStorageExtensions
{
    //"Data Source=yourdatabase.db;"
    public static void AddAddSqlLiteStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("SqlLite");
        services.AddDbContext<SqlLiteDbContext>(x => x.UseSqlite(cs));
        services.AddTransient<IEnvironmentService, SqlLiteEnvironmentService>();
        services.AddTransient<IDatabaseService, SqlLiteDatabaseService>();
        services.AddTransient<ITableService, SqlLiteTableService>();
        services.AddTransient<IColumnService, SqlLiteColumnService>();

        services.AddTransient<IColumnMetricsService, ColumnMetricsService>();
        services.AddTransient<ITableMetricsService, TableMetricsService>();
        services.AddTransient<IDatabaseMetricsService, DatabaseMetricsService>();
        services.AddTransient<IEnvMetricsService, EnvMetricsService>();
    }
}