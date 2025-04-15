using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace DBAudit.Infrastructure.SqlServer.Extensions;

public static class SqlServerExtensions
{
    public static void AddSqlServerProvider<TDbContext>(this IServiceCollection services, string connectionString) where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>(options => options.UseSqlServer(connectionString));
    }
}