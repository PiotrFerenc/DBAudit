using System.Threading.Channels;
using DBAudit.Infrastructure.Queue.Channels.Metrics;
using DBAudit.Infrastructure.Queue.Channels.StructureProcessors;
using Microsoft.Extensions.DependencyInjection;

namespace DBAudit.Infrastructure.Queue.Channels.Extensions;

public static class ChannelExtensions
{
    public static void AddChanelQueue(this IServiceCollection services)
    {
        services.AddSingleton<IQueueProvider, ChannelQueueProvider>();
        
        services.AddHostedService<CounterMetricsProcessor>();
        services.AddHostedService<UpdateDatabaseStructure>();
        services.AddHostedService<DatabasesAnalyzerProcessor>();

        var config = new UnboundedChannelOptions
        {
            AllowSynchronousContinuations = false,
            SingleReader = true,
            SingleWriter = true
        };

        services.AddSingleton<Channel<EnvironmentMessage>>(_ => Channel.CreateUnbounded<EnvironmentMessage>(config));
        
        services.AddSingleton<Channel<CounterMetricMessage>>(_ => Channel.CreateUnbounded<CounterMetricMessage>(config));
        
        services.AddSingleton<Channel<DatabaseAnalyzerMessage>>(_ => Channel.CreateUnbounded<DatabaseAnalyzerMessage>(config));
    }
}