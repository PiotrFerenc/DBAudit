using DBAudit.Analyzer;
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

        foreach (var table in tables.Where(table => !tableService.Exist(message.DbId, message.EnvId, table.Name)))
        {
            table.DatabaseId = message.DbId;
            table.EnvironmentId = message.EnvId;
            tableService.Add(table);
            await dispatcher.Send(new AnalyzeDatabase
            {
                EnvId = message.EnvId,
                DbId = message.DbId,
            });

            queueProvider.Enqueue(new ColumnsMessage(message.EnvId, message.DbId, table.Id));
        }
    }
}