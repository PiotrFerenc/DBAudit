using DBAudit.Infrastructure.Contracts.Entities;
using LanguageExt;

namespace DBAudit.Infrastructure.Storage.Binary;

public class CounterService(IDbAuditStorage<CounterDetails> storage) : ICounterService
{
    public void Add(CounterDetails counter) => storage.SaveItem(counter);

    public void Add(int value, string type, Guid envId)
    {
        Add(value, type, envId, Guid.Empty, Guid.Empty, Guid.Empty);
    }

    public void Add(int value, string type, Guid envId, Guid dbId)
    {
        Add(value, type, envId, dbId, Guid.Empty, Guid.Empty);
    }

    public void Add(int value, string type, Guid envId, Guid dbId, Guid tableId)
    {
        Add(value, type, envId, dbId, tableId, Guid.Empty);
    }

    public void Add(int value, string type, Guid envId, Guid dbId, Guid tableId, Guid columnId)
        => storage.SaveItem(new CounterDetails
        {
            Id = Guid.NewGuid(),
            Title = type,
            Value = value,
            Items = [],
            EnvironmentId = envId,
            DatabaseId = dbId,
            TableId = tableId,
            ColumnId = columnId
        });

    public int Count(string type, Guid envId) => Count(type, envId, Guid.Empty, Guid.Empty, Guid.Empty);
    public int Count(string type, Guid envId, Guid dbId) => Count(type, envId, dbId, Guid.Empty, Guid.Empty);
    public int Count(string type, Guid envId, Guid dbId, Guid tableId) => Count(type, envId, dbId, tableId, Guid.Empty);
    public int Count(string type, Guid envId, Guid dbId, Guid tableId, Guid columnId) => storage.Count(x => x.Type == type && x.EnvironmentId == envId && x.DatabaseId == dbId && x.TableId == tableId && x.ColumnId == columnId);


    public Option<CounterDetails> Get(Guid id) => storage.Find(x => x.Id == id);
    public void Remove(params Guid[] id) => storage.RemoveByKey(x => id.Contains(x.Id));
}