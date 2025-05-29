using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Storage.Metrics;

namespace DBAudit.Infrastructure.Storage.SqlLite.Metrics;

public class ColumnMetricsService(SqlLiteDbContext dbContext) : IMetricsService
{
    public void Add(Contracts.Entities.Metric counter)
    {
        dbContext.Metrics.Add(counter);
        dbContext.SaveChanges();
    }

    public List<Metric> Get(MetricKey key)
    {
        return dbContext.Metrics.Where(x => x.Key == key.Key).ToList();
    }

    public List<Metric> GetEnvMetrics(EnvName envName) =>
        dbContext.Metrics.Where(x => x.Key.EndsWith(envName.Value)).ToList();
    
}