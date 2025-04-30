using System.Threading.Channels;
using DBAudit.Infrastructure.DatabaseProvider;
using DBAudit.Infrastructure.Storage;
using Microsoft.Extensions.Hosting;

namespace DBAudit.Infrastructure.Queue.Channels;

public class EnvironmentProcessor(Channel<EnvironmentMessage> channel, IDatabaseProvider databaseProvider, IDatabaseService databaseService, IQueueProvider queueProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await channel.Reader.WaitToReadAsync(stoppingToken))
        {
            var message = await channel.Reader.ReadAsync(stoppingToken);
            var items = await databaseProvider.GetDatabases(message.Id);

            foreach (var database in items)
            {
                if (!databaseService.Exist(message.Id, database.Name))
                {
                    database.EnvironmentId = message.Id;
                    databaseService.Add(database);
                }

                queueProvider.Enqueue(new DatabaseMessage(message.Id, database.Id));
            }
        }
    }
}