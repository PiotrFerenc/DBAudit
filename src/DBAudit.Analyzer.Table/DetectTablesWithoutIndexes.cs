using DBAudit.Analyzer.Table.Common;
using DBAudit.Infrastructure;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Storage.Metrics;
using LanguageExt;
using Microsoft.Data.SqlClient;
using Environment = DBAudit.Infrastructure.Contracts.Entities.Environment;

namespace DBAudit.Analyzer.Table;

public class DetectTablesWithoutIndexes(SqlConnection connection, Environment env, Database database, Infrastructure.Contracts.Entities.Table table)
    : TableAnalyzer(connection, $"The table '{table.Name}' in database '{database.Name}' has no defined indexes.",env, database, table);
    
    
 
public class DetectTablesWithoutIndexesHandler(IQueryService queryService, IMetricsService tableMetricsService) : IRequestHandler<DetectTablesWithoutIndexes, Option<string>>
{
    public async Task<Option<string>> HandleAsync(DetectTablesWithoutIndexes request)
    {
        var query = GetQuery(request.table.Name);
        await queryService.QuerySingleData(request.connection, query, reader => (reader.GetInt32(0)))
            .IfSomeAsync(count =>
            {
                var key = new MetricKey(new TableName(request.table.Name), new DatabaseName(request.database.Name), new EnvName(request.env.Name));
                if (count != 0) tableMetricsService.Add(Metric.Create(request.name, "⚠️", nameof(DetectTablesWithoutIndexes), key));
            });

        return Option<string>.None;
    }
    
    private static string GetQuery(string tableName)=>$"""
        SELECT
            count(1)
        FROM
            sys.tables t
                JOIN sys.schemas s ON t.schema_id = s.schema_id
        WHERE
            NOT EXISTS (
                SELECT 1
                FROM sys.indexes i
                WHERE i.object_id = t.object_id
                  AND i.type > 0 
            )
        and t.name ='{tableName}`'
        """;
}
