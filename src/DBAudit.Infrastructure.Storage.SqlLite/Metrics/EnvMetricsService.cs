using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Storage.Metrics;

namespace DBAudit.Infrastructure.Storage.SqlLite.Metrics;

public class EnvMetricsService(SqlLiteDbContext dbContext) : IEnvMetricsService
{
    public void Add(MetricsDetails counter)
    {
        dbContext.EnvironmentMetrics.Add(counter);
        dbContext.SaveChanges();
    }

    public List<MetricsDetails> GetAllByEnvId(Guid envId)
    {
        return dbContext.EnvironmentMetrics.Where(x => x.EnvironmentId == envId).ToList();
    }
}