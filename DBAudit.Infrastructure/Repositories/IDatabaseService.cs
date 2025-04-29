using DBAudit.Infrastructure.Data.Entities;
using LanguageExt;

namespace DBAudit.Infrastructure.Repositories;

public interface ITableService
{
    void Add(Table table);
    List<Table> GetAll();
    List<Table> GetAll(Guid databaseId);
    bool Exist(Guid messageDbId, Guid messageEnvId);
    Option<Table> GetById(Guid tableId);
}

public class TableService(IBinaryStorage<Table> storage) : ITableService
{
    public void Add(Table table) => storage.SaveItem(table);
    public List<Table> GetAll() => storage.FetchAll();
    public List<Table> GetAll(Guid databaseId) => storage.Where(x => x.DatabaseId == databaseId);
    public bool Exist(Guid dbId, Guid envId) => storage.Find(x => x.DatabaseId == dbId && x.EnvironmentId == envId).IsSome;
    public Option<Table> GetById(Guid tableId) => storage.Find(x => x.Id == tableId);
}

public interface IDatabaseService
{
    void Add(Database database);
    void Activate(Guid id);
    void Deactivate(Guid id);
    void ChangeName(string id, string name);
    List<Database> GetAll(Guid envId);
    Option<Database> GetById(Guid id);
    bool Exist(Guid databaseId);
    bool Exist(Guid envId, string databaseName);
}

public class DatabaseService(IBinaryStorage<Database> storage) : IDatabaseService
{
    public void Add(Database database) => storage.SaveItem(database);
    public void Activate(Guid id) => storage.Update(d => d.IsActive = true, x => x.Id == id);
    public void Deactivate(Guid id) => storage.Update(d => d.IsActive = false, x => x.Id == id);
    public void ChangeName(string id, string name) => storage.Update(d => d.Name = name, d => d.Id == Guid.Parse(id));
    public List<Database> GetAll(Guid envId) => storage.Where(x => x.EnvironmentId == envId);
    public List<Database> GetAll() => storage.FetchAll();
    public Option<Database> GetById(Guid id) => storage.Find(x => x.Id == id);
    public bool Exist(Guid databaseId) => storage.Find(x => x.Id == databaseId).IsSome;
    public bool Exist(Guid envId, string databaseName) => storage.Find(x => x.EnvironmentId == envId && x.Name == databaseName).IsSome;
}