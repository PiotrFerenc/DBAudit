using DBAudit.Infrastructure.Contracts.Entities;
using LanguageExt;

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

    public Option<Table> Get(Guid messageDbId, Guid messageEnvId, string tableName)
    {
        return dbContext.Tables.FirstOrDefault(x => 
            x.DatabaseId == messageDbId && 
            x.EnvironmentId == messageEnvId && 
            x.Name == tableName);
    }

    public Option<Table> GetById(Guid tableId)
    {
        return dbContext.Tables.FirstOrDefault(x => x.Id == tableId);
    }
}