using DBAudit.Infrastructure.Contracts.Entities;
using LanguageExt;

namespace DBAudit.Infrastructure.Storage.Binary;

public class DatabaseService(IDbAuditStorage<Database> storage) : IDatabaseService
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
    public Option<Database> GetByName(Guid envId, string databaseName) => storage.Find(x => x.EnvironmentId == envId && x.Name == databaseName);
}