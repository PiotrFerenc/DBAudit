using DBAudit.Infrastructure.Contracts.Entities;

namespace DBAudit.Infrastructure.Storage.Metrics;

public interface IMetricsService
{
    void Add(Contracts.Entities.Metric counter);
    List<Contracts.Entities.Metric> Get(MetricKey key);
    List<Contracts.Entities.Metric> GetEnvMetrics(EnvName envName);
}