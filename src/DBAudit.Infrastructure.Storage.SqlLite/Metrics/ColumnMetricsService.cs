using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Storage.Metrics;

namespace DBAudit.Infrastructure.Storage.SqlLite.Metrics;

public class ColumnMetricsService(SqlLiteDbContext dbContext) : IColumnMetricsService
{
    public void Add(ColumnMetrics counter)
    {
        dbContext.ColumnMetrics.Add(counter);
        dbContext.SaveChanges();
    }

    public List<ColumnMetrics> GetAllByTableId(Guid tableId)
    {
        return dbContext.ColumnMetrics.Where(x => x.TableId == tableId).ToList();
    }
}