using DBAudit.Infrastructure.Data.Entities;

namespace DBAudit.Infrastructure;

public interface IDatabaseProvider
{
    Task<List<Database>> GetDatabases(Guid envId);
}