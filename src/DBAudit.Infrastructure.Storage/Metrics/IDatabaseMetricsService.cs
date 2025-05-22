using DBAudit.Infrastructure.Contracts.Entities;

namespace DBAudit.Infrastructure.Storage.Metrics;

public interface IDatabaseMetricsService
{
    void Add(MetricsDetails counter);
    List<MetricsDetails> GetAllByEnvId(Guid envId);
}