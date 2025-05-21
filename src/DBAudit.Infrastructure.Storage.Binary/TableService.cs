using DBAudit.Infrastructure.Contracts.Entities;
using LanguageExt;

namespace DBAudit.Infrastructure.Storage.Binary;

public class TableService(IDbAuditStorage<Table> storage) : ITableService
{
    public void Add(Table table) => storage.SaveItem(table);
    public List<Table> GetAll() => storage.FetchAll();
    public List<Table> GetAll(Guid databaseId) => storage.Where(x => x.DatabaseId == databaseId);
    public List<Table> GetAllByEnvId(Guid envId)=> storage.Where(x => x.EnvironmentId == envId);

    public Option<Table> Get(Guid dbId, Guid envId, string tableName) => storage.Find(x => x.DatabaseId == dbId && x.EnvironmentId == envId && x.Name == tableName);
    public Option<Table> GetById(Guid tableId) => storage.Find(x => x.Id == tableId);
}