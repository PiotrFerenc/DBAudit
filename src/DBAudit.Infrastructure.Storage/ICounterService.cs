using DBAudit.Infrastructure.Contracts.Entities;
using LanguageExt;

namespace DBAudit.Infrastructure.Storage;

public interface ICounterService
{
    void Add(CounterDetails counter);
    Option<CounterDetails> Get(Guid id);
    void Remove(params Guid[] id);
}