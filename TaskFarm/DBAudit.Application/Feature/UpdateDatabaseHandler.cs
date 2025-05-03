using DBAudit.Analyzer;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.DatabaseProvider;
using DBAudit.Infrastructure.Queue;
using DBAudit.Infrastructure.Storage;

namespace DBAudit.Application.Feature;

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