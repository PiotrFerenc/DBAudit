using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Storage.Metrics;
using Microsoft.EntityFrameworkCore;

namespace DBAudit.Infrastructure.Storage.SqlLite.Metrics;

public class MetricsService(SqlLiteDbContext dbContext) : IMetricsService
{
    public async Task Add(Contracts.Entities.Metric metric)
    {
         dbContext.Metrics.RemoveRange(await dbContext.Metrics.Where(x => x.Key == metric.Key && x.Type == metric.Type ).ToListAsync()); 
        dbContext.Metrics.Add(metric);
        await dbContext.SaveChangesAsync();
    }

    public  async Task<List<Metric>> Get(MetricKey key)=> await dbContext.Metrics.Where(x => x.Key == key.Key).ToListAsync();

    public async Task<List<Metric>> GetEnvMetrics(EnvName envName) =>
        await dbContext.Metrics.Where(x => x.Key.EndsWith(envName.Value)).ToListAsync();
    
}