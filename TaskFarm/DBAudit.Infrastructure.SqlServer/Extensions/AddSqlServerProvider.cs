using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace DBAudit.Infrastructure.SqlServer.Extensions;

public static class SqlServerExtensions
{
    public static void AddSqlServerProvider<TDbContext>(this IServiceCollection services, string connectionString) where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>(options => options.UseSqlServer(connectionString));
        
        services.AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddSqlServer() 
                .WithGlobalConnectionString(connectionString)  
                .ScanIn(typeof(Data.ApplicationDbContext).Assembly).For.Migrations()
            )
            .AddLogging(logging => logging.AddFluentMigratorConsole());  

    }
}