using DBAudit.Analyzer.Table.Common;
using DBAudit.Infrastructure;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Storage;
using DBAudit.Infrastructure.Storage.Metrics;
using LanguageExt;

namespace DBAudit.Analyzer.Table;

public class IsTableWithoutPrimaryKeysHandler(IQueryService queryService, IDatabaseService databaseService, IColumnMetricsService metricsService) : IRequestHandler<IsTableWithoutPrimaryKeys, Option<string>>
{
    public async Task<Option<string>> HandleAsync(IsTableWithoutPrimaryKeys request)
    {
        await databaseService.GetById(request.dbId).IfSomeAsync(async db =>
        {
            var query = QueryConstants.TablesWithoutPk.Replace("@table", db.Name);
            await queryService.QuerySingleData(request.connection, query, reader => (reader.GetInt32(0)))
                .IfSomeAsync(count => { metricsService.Add(MetricsDetails.CreateTableMetrics(request.name, count > 0 ? 1 : 0, request.envId, request.dbId, request.tableId, nameof(IsTableWithoutPrimaryKeys))); });
        });

        return Option<string>.None;
    }
}
