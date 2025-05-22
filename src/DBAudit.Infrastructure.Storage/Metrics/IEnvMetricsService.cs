using DBAudit.Infrastructure.Contracts.Entities;

namespace DBAudit.Infrastructure.Storage.Metrics;

public interface IEnvMetricsService
{
    void Add(MetricsDetails counter);
    List<MetricsDetails> GetAllByEnvId(Guid envId);
}