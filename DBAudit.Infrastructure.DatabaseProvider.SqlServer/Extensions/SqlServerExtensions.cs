using Microsoft.Extensions.DependencyInjection;

namespace DBAudit.Infrastructure.DatabaseProvider.SqlServer.Extensions;

public static class SqlServerExtensions
{
    public static void AddSqlServerProvider(this IServiceCollection services)
    {
        services.AddTransient<IDatabaseProvider, SqlServerProvider>();
    }
}