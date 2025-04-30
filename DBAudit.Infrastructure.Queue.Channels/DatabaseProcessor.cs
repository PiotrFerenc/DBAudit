using System.Threading.Channels;
using DBAudit.Analyzer;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.DatabaseProvider;
using DBAudit.Infrastructure.Storage;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;

namespace DBAudit.Infrastructure.Queue.Channels;

public class DatabaseProcessor(Channel<DatabaseMessage> channel, IDatabaseProvider databaseProvider, ITableService tableService, IQueueProvider queueProvider, IAnalyzerService tableAnalyzerService, ICommandDispatcher dispatcher) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await channel.Reader.WaitToReadAsync(stoppingToken))
        {
            var message = await channel.Reader.ReadAsync(stoppingToken);
            var tables = await databaseProvider.GetTables(message.EnvId, message.DbId);
            var cs = databaseProvider.GetConnectionString(message.EnvId, message.DbId);
            await cs.IfSomeAsync(async connectionString =>
            {
                var connection = new SqlConnection(connectionString);
                var analyzers = tableAnalyzerService.GetDatabaseAnalyzers(connection);
                foreach (var analyzer in analyzers)
                {
                    var result = await dispatcher.Send(analyzer);
                }
            });

            foreach (var table in tables)
            {
                if (tableService.Exist(message.DbId, message.EnvId)) continue;
                table.DatabaseId = message.DbId;
                table.EnvironmentId = message.EnvId;
                tableService.Add(table);
                queueProvider.Enqueue(new ColumnsMessage(message.EnvId, message.DbId, table.Id));
            }
        }
    }
}