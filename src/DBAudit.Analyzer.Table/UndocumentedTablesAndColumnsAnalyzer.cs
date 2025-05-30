using DBAudit.Infrastructure;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Storage.Metrics;
using LanguageExt;
using Microsoft.Data.SqlClient;
using Environment = DBAudit.Infrastructure.Contracts.Entities.Environment;

namespace DBAudit.Analyzer.Table;

public class UndocumentedTablesAndColumns(SqlConnection connection, Environment env, Database database, Infrastructure.Contracts.Entities.Table table)
    : TableAnalyzer(connection, $"In the database '{database.Name}', the table '{table.Name}' or its columns may be undocumented.", env, database, table);

public class UndocumentedTablesAndColumnsHandler(IQueryService queryService, IMetricsService metricsService) : IRequestHandler<UndocumentedTablesAndColumns, Option<string>>
{
    public async Task<Option<string>> HandleAsync(UndocumentedTablesAndColumns request)
    {
        var query = GetQuery(request.table.Name);
        await queryService.QuerySingleData(request.connection, query, reader => (reader.GetInt32(0)))
            .IfSomeAsync(count =>
            {
                var key = new MetricKey(new TableName(request.table.Name), new DatabaseName(request.database.Name), new EnvName(request.env.Name));
                if (count != 0) 
                {
                    metricsService.Add(Metric.Create(request.name, "⚠️", nameof(UndocumentedTablesAndColumns), key));
                }
            });

        return Option<string>.None;
    }

    private static string GetQuery(string tableName) => $"""
        SELECT Count(1)
        FROM sys.tables t
        LEFT JOIN sys.extended_properties ep
            ON t.object_id = ep.major_id
            AND ep.minor_id = 0
            AND ep.class = 1
        WHERE t.name = '{tableName}'
          AND ep.major_id IS NULL

          UNION ALL

        SELECT Count(1)
        FROM sys.columns c
        LEFT JOIN sys.extended_properties ep
            ON c.object_id = ep.major_id
            AND c.column_id = ep.minor_id
            AND ep.class = 1
        WHERE c.object_id = OBJECT_ID('{tableName}')
          AND ep.major_id IS NULL;
        """;
}