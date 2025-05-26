using DBAudit.Infrastructure.Contracts.Entities;

namespace DBAudit.Infrastructure.Storage.Metrics;

public interface IColumnMetricsService
{
    void Add(ColumnMetrics counter);
    List<ColumnMetrics> GetAllByTableId(Guid tableId);
}