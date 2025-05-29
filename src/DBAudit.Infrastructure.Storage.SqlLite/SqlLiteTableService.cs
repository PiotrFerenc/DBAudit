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

    public List<Table> GetAllByDbId(Guid dbId)
    {
        return dbContext.Tables.Where(x => x.DatabaseId == dbId).ToList();
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

}