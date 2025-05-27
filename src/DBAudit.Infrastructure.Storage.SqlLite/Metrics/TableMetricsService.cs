using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Storage.Metrics;
using Microsoft.EntityFrameworkCore;

namespace DBAudit.Infrastructure.Storage.SqlLite.Metrics;


public class TableMetricsService (SqlLiteDbContext dbContext): ITableMetricsService
{
    public void Add(TableMetrics counter)
    {
        var item = dbContext.TableMetrics.FirstOrDefault(e => e.TableId == counter.TableId && e.Type == counter.Type );
        if (item == null)
        {
            dbContext.TableMetrics.Add(counter);
        }
        else
        {
            item.Value = counter.Value;
            item.UpdatedAt = counter.UpdatedAt;
            dbContext.TableMetrics.Update(item);
        }
        dbContext.SaveChanges();
    }

    public List<TableMetrics> GetAllByDatabaseId(Guid databaseId)
    {
        return dbContext.TableMetrics.Where(x => x.DatabaseId == databaseId).ToList();
    }

    public List<TableMetrics> GetByTableId(Guid tableId)
    {
        return dbContext.TableMetrics.Where(x => x.TableId == tableId).ToList();
    }
}