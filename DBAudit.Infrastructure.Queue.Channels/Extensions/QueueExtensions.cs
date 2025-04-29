using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;

namespace DBAudit.Infrastructure.Queue.Channels.Extensions;

public static class ChannelExtensions
{
    public static void AddChanelQueue(this IServiceCollection services)
    {
        services.AddSingleton<IQueueProvider, ChannelQueueProvider>();
        services.AddHostedService<EnvironmentProcessor>();
        services.AddHostedService<DatabaseProcessor>();
        services.AddHostedService<TableProcessor>();

        services.AddSingleton<Channel<EnvironmentMessage>>(_ => Channel.CreateUnbounded<EnvironmentMessage>(new UnboundedChannelOptions
        {
            AllowSynchronousContinuations = false,
            SingleReader = true,
            SingleWriter = true
        }));

        services.AddSingleton<Channel<DatabaseMessage>>(_ => Channel.CreateUnbounded<DatabaseMessage>(new UnboundedChannelOptions
        {
            AllowSynchronousContinuations = false,
            SingleReader = true,
            SingleWriter = true
        }));

        services.AddSingleton<Channel<ColumnsMessage>>(_ => Channel.CreateUnbounded<ColumnsMessage>(new UnboundedChannelOptions
        {
            AllowSynchronousContinuations = false,
            SingleReader = true,
            SingleWriter = true
        }));
    }
}