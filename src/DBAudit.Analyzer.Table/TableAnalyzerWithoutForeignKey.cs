using DBAudit.Infrastructure;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Storage.Metrics;
using LanguageExt;
using Microsoft.Data.SqlClient;
using Environment = DBAudit.Infrastructure.Contracts.Entities.Environment;

namespace DBAudit.Analyzer.Table;

public class TableWithoutForeignKey(SqlConnection connection, Environment env, Database database, Infrastructure.Contracts.Entities.Table table)
    : TableAnalyzer(connection, $"In the database '{database.Name}', the table '{table.Name}' does not have any foreign key relationships.", env, database, table);

public class TableWithoutForeignKeyHandler(IQueryService queryService, IMetricsService tableMetricsService) : IRequestHandler<TableWithoutForeignKey, Option<string>>
{
    public async Task<Option<string>> HandleAsync(TableWithoutForeignKey request)
    {
        var query = GetQuery(request.table.Name);
        await queryService.QuerySingleData(request.connection, query, reader => (reader.GetInt32(0)))
            .IfSomeAsync(foreignKeyCount =>
            {
                if (foreignKeyCount == 0)
                {
                    var key = new MetricKey(new TableName(request.table.Name), new DatabaseName(request.database.Name), new EnvName(request.env.Name));
                    tableMetricsService.Add(Metric.Create(request.name, "⚠️", nameof(TableWithoutForeignKey), key));
                }
            });

        return Option<string>.None;
    }

    private static string GetQuery(string tableName) => $"""
                                                         SELECT COUNT(1)
                                                         FROM sys.foreign_keys fk
                                                         JOIN sys.tables t ON fk.parent_object_id = t.object_id
                                                         WHERE t.name = '{tableName}';
                                                         """;
}