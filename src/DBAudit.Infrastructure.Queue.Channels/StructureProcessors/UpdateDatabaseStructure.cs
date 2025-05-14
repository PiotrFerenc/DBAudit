using System.Threading.Channels;
using DBAudit.Infrastructure.Command;
using Microsoft.Extensions.Hosting;

namespace DBAudit.Infrastructure.Queue.Channels.StructureProcessors;

public class UpdateDatabaseStructure(Channel<EnvironmentMessage> channel, ICommandDispatcher dispatcher) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await channel.Reader.WaitToReadAsync(stoppingToken))
        {
            var message = await channel.Reader.ReadAsync(stoppingToken);
            await dispatcher.Send(message);
        }
    }
}