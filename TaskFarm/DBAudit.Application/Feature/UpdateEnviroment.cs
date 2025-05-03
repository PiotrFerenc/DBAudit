using DBAudit.Analyzer;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.DatabaseProvider;
using DBAudit.Infrastructure.Queue;
using DBAudit.Infrastructure.Storage;

namespace DBAudit.Application.Feature;

public class UpdateEnvironmentHandler(IDatabaseProvider databaseProvider, IDatabaseService databaseService, IQueueProvider queueProvider) : ICommandHandler<EnvironmentMessage>
{
    public async Task HandleAsync(EnvironmentMessage command)
    {
        var items = await databaseProvider.GetDatabases(command.Id);

        foreach (var database in items)
        {
            if (!databaseService.Exist(command.Id, database.Name))
            {
                database.EnvironmentId = command.Id;
                databaseService.Add(database);
            }

            queueProvider.Enqueue(new DatabaseMessage(command.Id, database.Id));
        }
    }
}

public class UpdateDatabaseHandler(IDatabaseProvider databaseProvider, ITableService tableService, IQueueProvider queueProvider, IAnalyzerService tableAnalyzerService) : ICommandHandler<DatabaseMessage>
{
    public async Task HandleAsync(DatabaseMessage message)
    {
        var tables = await databaseProvider.GetTables(message.EnvId, message.DbId);
        // var cs = databaseProvider.GetConnectionString(message.EnvId, message.DbId);
        // await cs.IfSomeAsync(async connectionString =>
        // {
        //     var connection = new SqlConnection(connectionString);
        //     var analyzers = tableAnalyzerService.GetDatabaseAnalyzers(connection);
        //     foreach (var analyzer in analyzers)
        //     {
        //         var result = await dispatcher.Send(analyzer);
        //     }
        // });

        foreach (var table in tables.Where(table => !tableService.Exist(message.DbId, message.EnvId, table.Name)))
        {
            table.DatabaseId = message.DbId;
            table.EnvironmentId = message.EnvId;
            tableService.Add(table);
            queueProvider.Enqueue(new ColumnsMessage(message.EnvId, message.DbId, table.Id));
        }
    }
}

public class UpdateTables(IDatabaseProvider databaseProvider, IColumnService columnService) : ICommandHandler<ColumnsMessage>
{
    public async Task HandleAsync(ColumnsMessage message)
    {
        var columns = await databaseProvider.GetColumns(message.EnvId, message.DbId, message.TableId);

        foreach (var column in columns)
        {
            columnService.Add(column);
        }
    }
}