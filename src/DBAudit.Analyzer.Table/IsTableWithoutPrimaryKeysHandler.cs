using DBAudit.Analyzer.Table.Common;
using DBAudit.Infrastructure;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Storage;

namespace DBAudit.Analyzer.Table;

public class IsTableWithoutPrimaryKeysHandler(IQueryService queryService, IDatabaseService databaseService) : ICommandHandler<IsTableWithoutPrimaryKeys>
{
    public async Task HandleAsync(IsTableWithoutPrimaryKeys request)
    {
        await databaseService.GetById(request.dbId).IfSomeAsync(db =>
        {
            var query = QueryConstants.TablesWithoutPk.Replace("@table", db.Name);
             queryService.QuerySingleData(request.connection, query, reader => (reader.GetInt32(0)))
                .IfSomeAsync(count =>
                {
                    var counterDetails = new CounterDetails
                    {
                        Title = request.name,
                        Value = count > 0 ? 1 : 0,
                        Id = Guid.NewGuid(),
                        Type = nameof(IsTableWithoutPrimaryKeys),
                        EnvironmentId = request.envId,
                        DatabaseId = request.dbId,
                        TableId = request.tableId,
                        ColumnId = Guid.Empty
                    };
                });
        });
    }
}