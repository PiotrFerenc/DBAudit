using DBAudit.Infrastructure.Contracts.Entities;

namespace DBAudit.Infrastructure.Storage.Metrics;

public interface IEnvMetricsService
{
    void Add(EnvironmentMetrics counter);
    List<EnvironmentMetrics> GetAllByEnvId(Guid envId);
}