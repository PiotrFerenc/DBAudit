using DBAudit.Infrastructure;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Storage.Metrics;
using LanguageExt;
using Microsoft.Data.SqlClient;
using Environment = DBAudit.Infrastructure.Contracts.Entities.Environment;

namespace DBAudit.Analyzer.Table;

public class ReportUnnecessaryTextBlobTypes(SqlConnection connection, Environment env, Database database, Infrastructure.Contracts.Entities.Table table)
    : TableAnalyzer(connection, $"In the database '{database.Name}', the table '{table.Name}' contains columns with TEXT/BLOB types without a valid need.", env, database, table);

public class ReportUnnecessaryTextBlobTypesHandler(IQueryService queryService, IMetricsService metricsService) : IRequestHandler<ReportUnnecessaryTextBlobTypes, Option<string>>
{
    public async Task<Option<string>> HandleAsync(ReportUnnecessaryTextBlobTypes request)
    {
        var query = GetQuery(request.table.Name);
        var columns = await queryService.QueryData(request.connection, query, reader => new
        {
            ColumnName = reader.GetString(0),
            DataType = reader.GetString(1)
        });
            if (columns.Any())
            {
                foreach (var column in columns)
                {
                var key = new MetricKey(new TableName(request.table.Name), new DatabaseName(request.database.Name), new EnvName(request.env.Name));
                   await metricsService.Add(Metric.Create(request.name, $"⚠️ Unnecessary use of {column.DataType} in column '{column.ColumnName}'", nameof(ReportUnnecessaryTextBlobTypes), key));
                }
            }

        return Option<string>.None;
    }

    private static string GetQuery(string tableName) => $"""
        SELECT c.name AS ColumnName, t.name AS DataType
        FROM sys.columns c
        JOIN sys.types t ON c.user_type_id = t.user_type_id
        WHERE t.name IN ('text', 'ntext', 'image', 'blob') AND OBJECT_NAME(c.object_id) = '{tableName}';
    """;
}