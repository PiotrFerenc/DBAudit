using DBAudit.Infrastructure.Contracts.Entities;
using LanguageExt;
using Microsoft.Data.SqlClient;

namespace DBAudit.Infrastructure.DatabaseProvider;

public interface IDatabaseProvider
{
    Task<List<string>> GetDatabases(SqlConnection connection);
    Task<List<string>> GetTables(SqlConnection connection);
    Task<IEnumerable<(string Type, string Name)>> GetColumns(string tableName, SqlConnection connection);
}