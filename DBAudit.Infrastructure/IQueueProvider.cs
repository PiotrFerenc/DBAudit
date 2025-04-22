namespace DBAudit.Infrastructure;

public interface IQueueProvider
{
    void Enqueue(EnvironmentMessage message);
}

public record EnvironmentMessage(Guid Id);