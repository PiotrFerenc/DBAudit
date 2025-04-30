namespace DBAudit.Infrastructure.Queue;

public interface IQueueProvider
{
    void Enqueue(EnvironmentMessage message);
    void Enqueue(DatabaseMessage message);
    void Enqueue(ColumnsMessage message);
}