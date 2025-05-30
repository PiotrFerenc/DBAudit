using DBAudit.Infrastructure;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Storage.Metrics;
using LanguageExt;
using Microsoft.Data.SqlClient;
using Environment = DBAudit.Infrastructure.Contracts.Entities.Environment;

namespace DBAudit.Analyzer.Table;

public class TableWithExcessiveColumnCount(SqlConnection connection, Environment env, Database database, Infrastructure.Contracts.Entities.Table table)
    : TableAnalyzer(connection, $"In the database '{database.Name}', the table '{table.Name}' has an excessive number of columns (>50).", env, database, table);
    

public class TableWithExcessiveColumnCountHandler(IQueryService queryService, IMetricsService tableMetricsService) : IRequestHandler<TableWithExcessiveColumnCount, Option<string>>
{
    public async Task<Option<string>> HandleAsync(TableWithExcessiveColumnCount request)
    {
        var query = GetQuery(request.table.Name);
        await queryService.QuerySingleData(request.connection, query, reader => (reader.GetInt32(0)))
            .IfSomeAsync(count =>
            {
                var key = new MetricKey(new TableName(request.table.Name), new DatabaseName(request.database.Name), new EnvName(request.env.Name));
                if (count > 50) tableMetricsService.Add(Metric.Create(request.name, "⚠️", nameof(TableWithExcessiveColumnCount), key));
            });

        return Option<string>.None;
    }

    private static string GetQuery(string tableName) => $"""
                                                         SELECT COUNT(1)
                                                         FROM sys.columns c
                                                         JOIN sys.tables t ON c.object_id = t.object_id
                                                         WHERE t.name = '{tableName}';
                                                         """;
}
