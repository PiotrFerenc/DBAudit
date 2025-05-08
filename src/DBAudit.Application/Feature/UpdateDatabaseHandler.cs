using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.DatabaseProvider;
using DBAudit.Infrastructure.Queue;
using DBAudit.Infrastructure.Storage;

namespace DBAudit.Application.Feature;

public class UpdateDatabaseHandler(IDatabaseProvider databaseProvider, ITableService tableService, IQueueProvider queueProvider, ICommandDispatcher dispatcher) : ICommandHandler<DatabaseMessage>
{
    public async Task HandleAsync(DatabaseMessage message)
    {
        var tables = await databaseProvider.GetTables(message.EnvId, message.DbId);

        foreach (var table in tables)
        {
            tableService.Get(message.DbId, message.EnvId, table.Name).Match(t =>
                {
                    queueProvider.Enqueue(new ColumnsMessage(t.EnvironmentId, t.DatabaseId, t.Id));
                },
                () =>
                {
                    table.DatabaseId = message.DbId;
                    table.EnvironmentId = message.EnvId;
                    tableService.Add(table);

                    queueProvider.Enqueue(new ColumnsMessage(table.DatabaseId, table.EnvironmentId, table.Id));
                }
            );
        }
    }
}