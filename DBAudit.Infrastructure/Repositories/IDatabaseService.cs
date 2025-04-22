using DBAudit.Infrastructure.Data.Entities;
using LanguageExt;

namespace DBAudit.Infrastructure.Repositories;

public interface ITableService
{
    void Add(Table table);
    List<Table> GetAll();
    List<Table> GetAll(Guid databaseId);
}

public class TableService(IStorage<Table> storage) : ITableService
{
    public void Add(Table table) => storage.SaveItem(table.Id.ToString(), table);
    public List<Table> GetAll() => storage.FetchAll();
    public List<Table> GetAll(Guid databaseId) => storage.Where(x => x.DatabaseId == databaseId);
}

public static class TableMapper
{
    public static Func<Table, string>[] MapToString() =>
    [
        p => p.Id.ToString(),
        p => p.Name,
        p => p.IsActive ? "1" : "0",
        p => p.DatabaseId.ToString(),
    ];

    public static Action<Table, string>[] MapFromString() =>
    [
        (p, b) => p.Id = Guid.Parse(b),
        (p, b) => p.Name = b,
        (p, b) => p.IsActive = b == "1",
        (p, b) => p.DatabaseId = Guid.Parse(b),
    ];
}

public interface IDatabaseService
{
    void Add(Database database);
    void Activate(string id);
    void Deactivate(string id);
    void Deactivate(Guid id);
    void ChangeName(string id, string name);
    List<Database> GetAll(Guid envId);
    Option<Database> GetById(string id);
}

public class DatabaseService(IStorage<Database> storage) : IDatabaseService
{
    public void Add(Database database) => storage.SaveItem(database.Id.ToString(), database);

    public void Activate(string id) => storage.Find(id).IfSome(d =>
    {
        d.IsActive = true;
        storage.UpdateItem(id, d);
    });

    public void Deactivate(string id) => storage.Find(id).IfSome(d =>
    {
        d.IsActive = false;
        storage.UpdateItem(id, d);
    });

    public void Deactivate(Guid id) => storage.UpdateMany(d => d.IsActive = false, d => d.EnvironmentId == id);
    public void ChangeName(string id, string name) => storage.UpdateItem(d => d.Name = name, d => d.Id == Guid.Parse(id));
    public List<Database> GetAll(Guid envId) => storage.Where(x => x.EnvironmentId == envId);

    public List<Database> GetAll() => storage.FetchAll();
    public Option<Database> GetById(string id) => storage.Find(id);
}

public static class DatabaseMapper
{
    public static Func<Database, string>[] MapToString() =>
    [
        p => p.Id.ToString(),
        p => p.Name,
        p => p.IsActive ? "1" : "0",
        p => p.EnvironmentId.ToString(),
    ];

    public static Action<Database, string>[] MapFromString() =>
    [
        (p, b) => p.Id = Guid.Parse(b),
        (p, b) => p.Name = b,
        (p, b) => p.IsActive = b == "1",
        (p, b) => p.EnvironmentId = Guid.Parse(b),
    ];
}