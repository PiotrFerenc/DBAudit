using DBAudit.Infrastructure.Contracts.Entities;

namespace DBAudit.Infrastructure.Storage.Metrics;

public interface IColumnMetricsService
{
    void Add(MetricsDetails counter);
    List<MetricsDetails> GetAllByTableId(Guid tableId);
}