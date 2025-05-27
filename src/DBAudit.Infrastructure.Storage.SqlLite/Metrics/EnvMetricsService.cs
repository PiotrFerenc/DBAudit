using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Storage.Metrics;

namespace DBAudit.Infrastructure.Storage.SqlLite.Metrics;

public class EnvMetricsService(SqlLiteDbContext dbContext) : IEnvMetricsService
{
    public void Add(EnvironmentMetrics counter)
    {
        var item = dbContext.EnvironmentMetrics.FirstOrDefault(x => x.Id == counter.Id);
        if (item == null)
        {
            dbContext.EnvironmentMetrics.Add(counter);
        }
        else
        {
           item.Value  = counter.Value;
           item.UpdatedAt  = DateTime.UtcNow;
        }
        dbContext.SaveChanges();
    }

    public List<EnvironmentMetrics> GetAllByEnvId(Guid envId)
    {
        return dbContext.EnvironmentMetrics.Where(x => x.EnvironmentId == envId).ToList();
    }
}