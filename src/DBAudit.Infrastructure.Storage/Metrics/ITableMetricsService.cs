using DBAudit.Infrastructure.Contracts.Entities;

namespace DBAudit.Infrastructure.Storage.Metrics;

public interface ITableMetricsService
{
    void Add(TableMetrics counter);
    List<TableMetrics> GetAllByDatabaseId(Guid databaseId);
    List<TableMetrics> GetByTableId(Guid tableId);
}