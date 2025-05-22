using DBAudit.Infrastructure.Contracts.Entities;

namespace DBAudit.Infrastructure.Storage.Metrics;

public interface ITableMetricsService
{
    void Add(MetricsDetails counter);
    List<MetricsDetails> GetAllByDatabaseId(Guid databaseId);
}