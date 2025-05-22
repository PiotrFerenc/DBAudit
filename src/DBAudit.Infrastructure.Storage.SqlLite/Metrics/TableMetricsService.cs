using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Storage.Metrics;

namespace DBAudit.Infrastructure.Storage.SqlLite.Metrics;


public class TableMetricsService (SqlLiteDbContext dbContext): ITableMetricsService
{
    public void Add(MetricsDetails counter)
    {
        dbContext.TableMetrics.Add(counter);
        dbContext.SaveChanges();
    }

    public List<MetricsDetails> GetAllByDatabaseId(Guid databaseId)
    {
        return dbContext.TableMetrics.Where(x => x.DatabaseId == databaseId).ToList();
    }
}