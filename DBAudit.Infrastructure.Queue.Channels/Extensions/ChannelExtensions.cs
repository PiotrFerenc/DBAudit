using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;

namespace DBAudit.Infrastructure.Queue.Channels.Extensions;

public static class ChannelExtensions
{
    public static void AddChanelQueue(this IServiceCollection services)
    {
        services.AddSingleton<IQueueProvider, ChannelQueueProvider>();
        
        services.AddHostedService<CounterMetricsProcessor>();
        services.AddHostedService<EnvironmentProcessor>();
        services.AddHostedService<DatabaseProcessor>();
        services.AddHostedService<TableProcessor>();

        var config = new UnboundedChannelOptions
        {
            AllowSynchronousContinuations = false,
            SingleReader = true,
            SingleWriter = true
        };

        services.AddSingleton<Channel<EnvironmentMessage>>(_ => Channel.CreateUnbounded<EnvironmentMessage>(config));
        services.AddSingleton<Channel<DatabaseMessage>>(_ => Channel.CreateUnbounded<DatabaseMessage>(config));
        services.AddSingleton<Channel<ColumnsMessage>>(_ => Channel.CreateUnbounded<ColumnsMessage>(config));
        services.AddSingleton<Channel<CounterMetricMessage>>(_ => Channel.CreateUnbounded<CounterMetricMessage>(config));
    }
}