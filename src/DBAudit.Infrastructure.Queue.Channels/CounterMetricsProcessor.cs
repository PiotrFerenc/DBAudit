using System.Threading.Channels;
using DBAudit.Analyzer;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Storage;
using Microsoft.Extensions.Hosting;

namespace DBAudit.Infrastructure.Queue.Channels;

public class CounterMetricsProcessor(Channel<CounterMetricMessage> channel, IReportService reportService, IAnalyzerService analyzerService, ICommandDispatcher dispatcher) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await channel.Reader.WaitToReadAsync(stoppingToken))
        {
            var message = await channel.Reader.ReadAsync(stoppingToken);

            var analyzers = analyzerService.GetDatabaseCounters(message.connection);

            foreach (var analyzer in analyzers)
            {
                await dispatcher.Send(analyzer).IfSomeAsync(value =>
                {
                    reportService.Get(message.messageDbId).IfSome(report =>
                        reportService.AddCounter(report.DatabaseId, (analyzer.Name, value.ToString()))
                    );
                });
            }
        }
    }
}