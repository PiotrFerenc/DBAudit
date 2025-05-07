using DBAudit.Infrastructure.Contracts.Entities;
using Microsoft.Extensions.DependencyInjection;
using Environment = DBAudit.Infrastructure.Contracts.Entities.Environment;

namespace DBAudit.Infrastructure.Storage.Binary;

public static class StorageExtensions
{
    public static void AddBinaryStorage(this IServiceCollection services)
    {
        services.AddSingleton<IDbAuditStorage<Environment>>(new BinaryStorage<Environment>($"{nameof(Environment)}.bin"));
        services.AddSingleton<IDbAuditStorage<Database>>(new BinaryStorage<Database>($"{nameof(Database)}.bin"));
        services.AddSingleton<IDbAuditStorage<Table>>(new BinaryStorage<Table>($"{nameof(Table)}.bin"));
        services.AddSingleton<IDbAuditStorage<Column>>(new BinaryStorage<Column>($"{nameof(Column)}.bin"));
        services.AddSingleton<IDbAuditStorage<ReportView>>(new BinaryStorage<ReportView>($"{nameof(ReportView)}.bin"));
        
        services.AddTransient<IEnvironmentService, EnvironmentService>();
        services.AddTransient<IDatabaseService, DatabaseService>();
        services.AddTransient<ITableService, TableService>();
        services.AddTransient<IColumnService, ColumnService>();
        services.AddTransient<IReportService, ReportService>();
    }
}