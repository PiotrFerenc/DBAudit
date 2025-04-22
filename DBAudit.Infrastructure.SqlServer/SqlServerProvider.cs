using DBAudit.Infrastructure.Data.Entities;

namespace DBAudit.Infrastructure.SqlServer;

public class SqlServerProvider : IDatabaseProvider
{
    public async Task<List<Database>> GetDatabases(Guid envId) => await Task.FromResult(new List<Database>());
}