using DBAudit.Analyzer.Table.Common;
using DBAudit.Infrastructure;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Storage;
using DBAudit.Infrastructure.Storage.Metrics;
using LanguageExt;

namespace DBAudit.Analyzer.Table;

public class IsTableWithoutPrimaryKeysHandler(IQueryService queryService, IMetricsService tableMetricsService) : IRequestHandler<IsTableWithoutPrimaryKeys, Option<string>>
{
    public async Task<Option<string>> HandleAsync(IsTableWithoutPrimaryKeys request)
    {
            var query = QueryConstants.TablesWithoutPk.Replace("@table", request.table.Name);
            await queryService.QuerySingleData(request.connection, query, reader => (reader.GetInt32(0)))
                .IfSomeAsync(count =>
                {
                    var key = new MetricKey(new TableName(request.table.Name), new DatabaseName(request.database.Name), new EnvName(request.env.Name));
                    if (count != 0) tableMetricsService.Add(Metric.Create(request.name, "⚠️", nameof(IsTableWithoutPrimaryKeys), key));
                });

        return Option<string>.None;
    }
}
