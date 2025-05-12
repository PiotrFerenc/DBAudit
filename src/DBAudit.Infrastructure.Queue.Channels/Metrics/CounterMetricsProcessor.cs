using System.Threading.Channels;
using DBAudit.Analyzer;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Storage;
using Microsoft.Extensions.Hosting;

namespace DBAudit.Infrastructure.Queue.Channels.Metrics;

public class CounterMetricsProcessor(Channel<CounterMetricMessage> channel, IAnalyzerService analyzerService, ICommandDispatcher dispatcher) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await channel.Reader.WaitToReadAsync(stoppingToken))
        {
            var message = await channel.Reader.ReadAsync(stoppingToken);

            var analyzers = analyzerService.GetDatabaseCounters(message.connection, message.reportId);

            foreach (var analyzer in analyzers)
            {
                await dispatcher.Send(analyzer);
            }
        }
    }
}