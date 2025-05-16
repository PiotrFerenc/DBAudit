using DBAudit.Infrastructure.Contracts.Entities;
using LanguageExt;

namespace DBAudit.Infrastructure.Storage;

public interface IMetricsService
{
    void Add(MetricsDetails counter);
    void Add(int value, string type, Guid envId);
    void Add(int value, string type, Guid envId, Guid dbId);
    void Add(int value, string type, Guid envId, Guid dbId, Guid tableId);
    void Add(int value, string type, Guid envId, Guid dbId, Guid tableId, Guid columnId);

    int Count(string type, Guid envId);
    int Count(string type, Guid envId, Guid dbId);
    int Count(string type, Guid envId, Guid dbId, Guid tableId);
    int Count(string type, Guid envId, Guid dbId, Guid tableId, Guid columnId);

    Option<MetricsDetails> Get(Guid id);
    void Remove(params Guid[] id);
}