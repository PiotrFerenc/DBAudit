using DBAudit.Infrastructure.Data.Entities;
using LanguageExt;

namespace DBAudit.Infrastructure;

public interface IDatabaseProvider
{
    Task<List<Database>> GetDatabases(Guid envId);
    Task<List<Table>> GetTables(Guid envId, Guid dbId);
    Task<bool> CheckConnection(Guid envId);
    Task<IEnumerable<Column>> GetColumns(Guid envId, Guid dbId, Guid tableId);
    Option<string> GetConnectionString(Guid envId, Guid dbId);
}