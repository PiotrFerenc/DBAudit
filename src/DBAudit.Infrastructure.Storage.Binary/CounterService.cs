using DBAudit.Infrastructure.Contracts.Entities;
using LanguageExt;

namespace DBAudit.Infrastructure.Storage.Binary;

public class CounterService(IDbAuditStorage<CounterDetails> storage) : ICounterService
{
    public void Add(CounterDetails counter) => storage.SaveItem(counter);
    public Option<CounterDetails> Get(Guid id) => storage.Find(x => x.Id == id);
    public void Remove(params Guid[] id) => storage.RemoveByKey(x => id.Contains(x.Id));   
}