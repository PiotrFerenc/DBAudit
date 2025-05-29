using DBAudit.Infrastructure.Contracts.Entities;
using LanguageExt;

namespace DBAudit.Infrastructure.Storage;

public interface ITableService
{
    void Add(Table table);
    List<Table> GetAll();
    List<Table> GetAll(Guid databaseId);
    List<Table> GetAllByEnvId(Guid envId);
    List<Table> GetAllByDbId(Guid envId);
    Option<Table> Get(Guid messageDbId, Guid messageEnvId, string tableName);
    Option<Table> GetById(Guid tableId);
}

public class CountMetric
{
    public string Type { get; set; }
    public string Value { get; set; }
    public string Title { get; set; }
}