using DBAudit.Infrastructure.Contracts.Entities;
using LanguageExt;

namespace DBAudit.Infrastructure.Storage;

public interface ITableService
{
    void Add(Table table);
    List<Table> GetAll();
    List<Table> GetAll(Guid databaseId);
    bool Exist(Guid messageDbId, Guid messageEnvId, string tableName);
    Option<Table> GetById(Guid tableId);
}