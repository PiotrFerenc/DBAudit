using System.Threading.Channels;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Storage;
using Microsoft.Extensions.Hosting;

namespace DBAudit.Infrastructure.Queue.Channels;

public class EnvironmentProcessor(Channel<EnvironmentMessage> channel, ICommandDispatcher dispatcher) : BackgroundService
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