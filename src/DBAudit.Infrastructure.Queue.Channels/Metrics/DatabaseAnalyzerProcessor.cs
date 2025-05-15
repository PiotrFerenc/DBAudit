using System.Threading.Channels;
using DBAudit.Application.Feature.Analyze;
using DBAudit.Infrastructure.Command;
using Microsoft.Extensions.Hosting;

namespace DBAudit.Infrastructure.Queue.Channels.Metrics;

public class DatabasesAnalyzerProcessor(Channel<DatabaseAnalyzerMessage> channel, ICommandDispatcher dispatcher) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await channel.Reader.WaitToReadAsync(stoppingToken))
        {
            var message = await channel.Reader.ReadAsync(stoppingToken);
            await dispatcher.Send(new AnalyzeEnvironment(message.EnvId));
        }
    }
}