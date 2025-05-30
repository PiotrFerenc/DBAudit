using DBAudit.Analyzer.Table.Common;
using DBAudit.Infrastructure;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Storage.Metrics;
using LanguageExt;

namespace DBAudit.Analyzer.Table;

public class TableWithoutPrimaryKeysHandler(IQueryService queryService, IMetricsService tableMetricsService) : IRequestHandler<TableWithoutPrimaryKeys, Option<string>>
{
    public async Task<Option<string>> HandleAsync(TableWithoutPrimaryKeys request)
    {
            var query = QueryConstants.TablesWithoutPk.Replace("@table", request.table.Name);
            await queryService.QuerySingleData(request.connection, query, reader => (reader.GetInt32(0)))
                .IfSomeAsync(count =>
                {
                    var key = new MetricKey(new TableName(request.table.Name), new DatabaseName(request.database.Name), new EnvName(request.env.Name));
                    if (count != 0) tableMetricsService.Add(Metric.Create(request.name, "⚠️", nameof(TableWithoutPrimaryKeys), key));
                });

        return Option<string>.None;
    }
}
