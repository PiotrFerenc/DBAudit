using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Storage.Metrics;

namespace DBAudit.Infrastructure.Storage.SqlLite.Metrics;

public class DatabaseMetricsService(SqlLiteDbContext dbContext) : IDatabaseMetricsService
{
    public void Add(DatabaseMetrics counter)
    {
        var item = dbContext.DatabaseMetrics.FirstOrDefault(e => e.DatabaseId == counter.DatabaseId && e.Type == counter.Type );
        if (item == null)
        {
            dbContext.DatabaseMetrics.Add(counter);
        }
        else
        {
            item.Value = counter.Value;
            item.UpdatedAt = counter.UpdatedAt;
            dbContext.DatabaseMetrics.Update(item);
        }
        
        dbContext.SaveChanges();
    }

    public List<DatabaseMetrics> GetAllByEnvId(Guid envId)
    {
        return dbContext.DatabaseMetrics.Where(x => x.EnvironmentId == envId).ToList();
    }
}