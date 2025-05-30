using DBAudit.Infrastructure;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Storage.Metrics;
using LanguageExt;
using Microsoft.Data.SqlClient;
using Environment = DBAudit.Infrastructure.Contracts.Entities.Environment;

namespace DBAudit.Analyzer.Table;

public class TableWithoutPrimaryKeys(SqlConnection connection, Environment env, Database database, Infrastructure.Contracts.Entities.Table table)
    : TableAnalyzer(connection, $"In the database '{database.Name}', the table '{table.Name}' does not have a primary key.",   env, database, table);
    
    
public class TableWithoutPrimaryKeysHandler(IQueryService queryService, IMetricsService tableMetricsService) : IRequestHandler<TableWithoutPrimaryKeys, Option<string>>
{
    public async Task<Option<string>> HandleAsync(TableWithoutPrimaryKeys request)
    {
        var query = GetQuery(request.table.Name);
        await queryService.QuerySingleData(request.connection, query, reader => (reader.GetInt32(0)))
            .IfSomeAsync(count =>
            {
                var key = new MetricKey(new TableName(request.table.Name), new DatabaseName(request.database.Name), new EnvName(request.env.Name));
                if (count != 0) tableMetricsService.Add(Metric.Create(request.name, "⚠️", nameof(TableWithoutPrimaryKeys), key));
            });

        return Option<string>.None;
    }
    private static string GetQuery(string tableName)=>$"""
                                          SELECT Count(1)
                                          FROM sys.tables t
                                          WHERE NOT EXISTS (SELECT 1
                                                            FROM sys.indexes i
                                                            WHERE i.object_id = t.object_id
                                                              AND i.is_primary_key = 1) and name = '{tableName}';
                                          """;
}
