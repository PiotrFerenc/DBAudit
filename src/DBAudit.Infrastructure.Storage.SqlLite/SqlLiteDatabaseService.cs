using DBAudit.Infrastructure.Contracts.Entities;
using LanguageExt;

namespace DBAudit.Infrastructure.Storage.SqlLite;

public class SqlLiteDatabaseService(SqlLiteDbContext dbContext) : IDatabaseService
{
    public void Add(Database database)
    {
        dbContext.Databases.Add(database);
        dbContext.SaveChanges();
    }

    public void Activate(Guid id)
    {
        var database = dbContext.Databases.Find(id);
        if (database == null) return;
        database.IsActive = true;
        dbContext.SaveChanges();
    }

    public void Deactivate(Guid id)
    {
        var database = dbContext.Databases.Find(id);
        if (database == null) return;
        database.IsActive = false;
        dbContext.SaveChanges();
    }

    public void ChangeName(string id, string name)
    {
        var database = dbContext.Databases.Find(Guid.Parse(id));
        if (database != null)
        {
            database.Name = name;
            dbContext.SaveChanges();
        }
    }

    public List<Database> GetAll(Guid envId) =>
        dbContext.Databases.Where(x => x.EnvironmentId == envId).ToList();

    public Option<Database> GetById(Guid id) => dbContext.Databases.Find(id);

    public bool Exist(Guid databaseId) =>
        dbContext.Databases.Any(x => x.Id == databaseId);

    public bool Exist(Guid envId, string databaseName) =>
        dbContext.Databases.Any(x => x.EnvironmentId == envId && x.Name == databaseName);

    public Option<Database> GetByName(Guid envId, string databaseName) =>
        dbContext.Databases.FirstOrDefault(x => x.EnvironmentId == envId && x.Name == databaseName);
}