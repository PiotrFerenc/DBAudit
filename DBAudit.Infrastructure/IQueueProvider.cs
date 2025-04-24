namespace DBAudit.Infrastructure;

public interface IQueueProvider
{
    void Enqueue(EnvironmentMessage message);
    void Enqueue(DatabaseMessage message);
    void Enqueue(ColumnsMessage message);
}

public record EnvironmentMessage(Guid Id);

public record DatabaseMessage(Guid EnvId, Guid DbId);

public record ColumnsMessage(Guid EnvId, Guid DbId, string TableName);