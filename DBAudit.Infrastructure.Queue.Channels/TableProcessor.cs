using System.Threading.Channels;
using DBAudit.Infrastructure.DatabaseProvider;
using DBAudit.Infrastructure.Storage;
using Microsoft.Extensions.Hosting;

namespace DBAudit.Infrastructure.Queue.Channels;

public class TableProcessor(Channel<ColumnsMessage> channel, IDatabaseProvider databaseProvider, IColumnService columnService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await channel.Reader.WaitToReadAsync(stoppingToken))
        {
            var message = await channel.Reader.ReadAsync(stoppingToken);
            var columns = await databaseProvider.GetColumns(message.EnvId, message.DbId, message.TableId);

            foreach (var column in columns)
            {
                columnService.Add(column);
            }
        }
    }
}