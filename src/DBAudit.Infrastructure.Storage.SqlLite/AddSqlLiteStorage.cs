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
    }
}