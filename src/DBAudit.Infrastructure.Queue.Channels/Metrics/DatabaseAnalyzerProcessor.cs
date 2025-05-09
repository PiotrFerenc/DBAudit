using System.Threading.Channels;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Storage;
using Microsoft.Extensions.Hosting;

namespace DBAudit.Infrastructure.Queue.Channels.Metrics;

public class DatabaseAnalyzerProcessor(Channel<DatabaseAnalyzerMessage> channel, ICommandDispatcher dispatcher, IDatabaseService databaseService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await channel.Reader.WaitToReadAsync(stoppingToken))
        {
            var message = await channel.Reader.ReadAsync(stoppingToken);
            var databases = databaseService.GetAll(message.EnvId);
            foreach (var database in databases)
            {
                await dispatcher.Send(new AnalyzeDatabase
                {
                    EnvId = database.EnvironmentId,
                    DbId = database.Id
                });
            }
        }
    }
}