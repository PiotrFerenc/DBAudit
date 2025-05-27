using DBAudit.Infrastructure.Contracts.Entities;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace DBAudit.Infrastructure.Storage.SqlLite;


public class SqlLiteTableService (SqlLiteDbContext dbContext): ITableService
{
    public void Add(Table table)
    {
        dbContext.Tables.Add(table);
        dbContext.SaveChanges();
    }

    public List<Table> GetAll()
    {
        return dbContext.Tables.ToList();
    }

    public List<Table> GetAll(Guid databaseId)
    {
        return dbContext.Tables.Where(x => x.DatabaseId == databaseId).ToList();
    }

    public List<Table> GetAllByEnvId(Guid envId)
    {
        return dbContext.Tables.Where(x => x.EnvironmentId == envId).ToList();
    }

    public Option<Table> Get(Guid dbId, Guid envId, string tableName)
    {
        return dbContext.Tables.FirstOrDefault(x => 
            x.DatabaseId == dbId && 
            x.EnvironmentId == envId && 
            x.Name == tableName);
    }

    public Option<Table> GetById(Guid tableId)
    {
        return dbContext.Tables.FirstOrDefault(x => x.Id == tableId);
    }

    public async Task<List<CountMetric>> CountMetrics(Guid envId, Guid tableId)
    {
        var results = await dbContext.TableMetrics
            .Where(x => x.EnvironmentId == envId && x.DatabaseId == tableId)
            .GroupBy(x => new { x.Type, x.Title })
            .Select(g => new CountMetric
            {
                Type = g.Key.Type,
                Title = g.Key.Title,
                Value = g.Sum(x => x.Value)
            }).ToListAsync();
return results;
    } }