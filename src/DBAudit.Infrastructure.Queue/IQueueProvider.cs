namespace DBAudit.Infrastructure.Queue;

public interface IQueueProvider
{
    void Enqueue(UpdateEnvironment message);
    void Enqueue(CounterMetricMessage message);
    void Enqueue(DatabaseAnalyzerMessage message);
}