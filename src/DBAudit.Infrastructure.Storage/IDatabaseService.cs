using DBAudit.Infrastructure.Contracts.Entities;
using LanguageExt;

namespace DBAudit.Infrastructure.Storage;

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
    Option<Database> GetByName(Guid envId, string databaseName);
    Task<List<CountMetric>> CountMetrics(Guid envId);
}