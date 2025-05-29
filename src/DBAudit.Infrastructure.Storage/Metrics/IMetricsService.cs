using DBAudit.Infrastructure.Contracts.Entities;

namespace DBAudit.Infrastructure.Storage.Metrics;

public interface IMetricsService
{
   Task Add(Metric counter);
    Task<List<Metric>> Get(MetricKey key);
    Task<List<Metric>> GetEnvMetrics(EnvName envName);
}