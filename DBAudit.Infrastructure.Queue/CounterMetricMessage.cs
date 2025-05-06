using DBAudit.Infrastructure.Command;

namespace DBAudit.Infrastructure.Queue;

public record CounterMetricMessage(Guid DbId, string MetricDescription, int value) : IRequest;