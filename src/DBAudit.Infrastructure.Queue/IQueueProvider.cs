namespace DBAudit.Infrastructure.Queue;

public interface IQueueProvider
{
    void Enqueue(EnvironmentMessage message);
    void Enqueue(CounterMetricMessage message);
    void Enqueue(DatabaseAnalyzerMessage message);
}