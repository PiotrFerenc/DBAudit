using System.Threading.Channels;
using DBAudit.Infrastructure.Command;
using Microsoft.Extensions.Hosting;

namespace DBAudit.Infrastructure.Queue.Channels;

public class TableProcessor(Channel<ColumnsMessage> channel, ICommandDispatcher dispatcher) : BackgroundService
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

public class CounterMetricsProcessor(Channel<CounterMetricMessage> channel) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await channel.Reader.WaitToReadAsync(stoppingToken))
        {
            //save counter
            var t = 1;
        }
    }
}