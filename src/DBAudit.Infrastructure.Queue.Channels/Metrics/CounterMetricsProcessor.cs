using System.Threading.Channels;
using DBAudit.Analyzer;
using DBAudit.Infrastructure.Command;
using Microsoft.Extensions.Hosting;

namespace DBAudit.Infrastructure.Queue.Channels.Metrics;

public class CounterMetricsProcessor(Channel<CounterMetricMessage> channel, ITableAnalyzerService analyzerService, ICommandDispatcher dispatcher) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await channel.Reader.WaitToReadAsync(stoppingToken))
        {
            var message = await channel.Reader.ReadAsync(stoppingToken);
            //
            // var analyzers = analyzerService.GetCheckAnalyzers(message.connection, message.reportId);
            //
            // foreach (var analyzer in analyzers)
            // {
            //     await dispatcher.Send(analyzer);
            // }
        }
    }
}