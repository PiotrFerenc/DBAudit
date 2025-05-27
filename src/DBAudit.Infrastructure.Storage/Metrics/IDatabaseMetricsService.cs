using DBAudit.Infrastructure.Contracts.Entities;

namespace DBAudit.Infrastructure.Storage.Metrics;

public interface IDatabaseMetricsService
{
    void Add(DatabaseMetrics counter);
    List<DatabaseMetrics> GetAllByEnvId(Guid envId);
}