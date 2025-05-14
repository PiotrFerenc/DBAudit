using System.Threading.Channels;

namespace DBAudit.Infrastructure.Queue.Channels;

internal class ChannelQueueProvider(Channel<EnvironmentMessage> envChannel, Channel<CounterMetricMessage> counterMetricChannel, Channel<DatabaseAnalyzerMessage> databaseAnalyzerChannel) : IQueueProvider
{
    public void Enqueue(EnvironmentMessage message)
    {
        envChannel.Writer.TryWrite(message);
    }


    public void Enqueue(CounterMetricMessage message)
    {
        counterMetricChannel.Writer.TryWrite(message);
    }

    public void Enqueue(DatabaseAnalyzerMessage message)
    {
        databaseAnalyzerChannel.Writer.TryWrite(message);
    }
}