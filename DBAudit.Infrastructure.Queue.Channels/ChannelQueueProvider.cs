using System.Threading.Channels;

namespace DBAudit.Infrastructure.Queue.Channels;

internal class ChannelQueueProvider(Channel<EnvironmentMessage> envChannel, Channel<DatabaseMessage> databaseChannel, Channel<ColumnsMessage> columnChannel) : IQueueProvider
{
    public void Enqueue(EnvironmentMessage message)
    {
        envChannel.Writer.TryWrite(message);
    }

    public void Enqueue(DatabaseMessage message)
    {
        databaseChannel.Writer.TryWrite(message);
    }

    public void Enqueue(ColumnsMessage message)
    {
        columnChannel.Writer.TryWrite(message);
    }
}