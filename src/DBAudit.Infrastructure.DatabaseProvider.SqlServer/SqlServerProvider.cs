using DBAudit.Infrastructure.Storage;
using Microsoft.Data.SqlClient;

namespace DBAudit.Infrastructure.DatabaseProvider.SqlServer;

public class SqlServerProvider(IQueryService queryService, IEnvironmentService environmentService)
    : IDatabaseProvider
{

    public async Task<List<string>> GetDatabases(SqlConnection connection) =>
        await queryService.QueryData(connection, "SELECT name FROM sys.databases WHERE database_id > 4", r => r.GetString(0));

    public async Task<List<string>> GetTables(SqlConnection connection)
        => await queryService.QueryData(connection, "SELECT table_name FROM information_schema.tables WHERE table_type = 'BASE TABLE';", r => r.GetString(0));

    public async Task<IEnumerable<(string Type, string Name)>> GetColumns(string tableName, SqlConnection connection)
        => await queryService.QueryData(connection, $@"SELECT  DATA_TYPE ,COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'",
            reader => (reader.GetString(0), reader.GetString(1))
        );
}