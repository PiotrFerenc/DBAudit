using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Storage.Metrics;

namespace DBAudit.Infrastructure.Storage.SqlLite.Metrics;

public class DatabaseMetricsService(SqlLiteDbContext dbContext) : IDatabaseMetricsService
{
    public void Add(MetricsDetails counter)
    {
        dbContext.DatabaseMetrics.Add(counter);
        dbContext.SaveChanges();
    }

    public List<MetricsDetails> GetAllByEnvId(Guid envId)
    {
        return dbContext.DatabaseMetrics.Where(x => x.EnvironmentId == envId).ToList();
    }
}