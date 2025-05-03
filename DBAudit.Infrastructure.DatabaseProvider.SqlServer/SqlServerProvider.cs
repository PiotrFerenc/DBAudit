using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Storage;
using LanguageExt;
using Microsoft.Data.SqlClient;
using Database = DBAudit.Infrastructure.Contracts.Entities.Database;

namespace DBAudit.Infrastructure.DatabaseProvider.SqlServer;

public class SqlServerProvider : IDatabaseProvider
{
    private readonly IEnvironmentService _environmentService;
    private readonly IDatabaseService _databaseService;
    private readonly ITableService _tableService;

    public SqlServerProvider(IEnvironmentService environmentService, IDatabaseService databaseService, ITableService tableService)
    {
        _environmentService = environmentService;
        _databaseService = databaseService;
        _tableService = tableService;
    }

    public async Task<List<Database>> GetDatabases(Guid envId)
    {
        var result = new List<Database>();
        var connectionString = _environmentService.GetConnectionString(envId);
        if (connectionString.IsSome)
        {
            var cs = string.Empty;
            connectionString.IfSome(c => cs = c);
            await using var connection = new SqlConnection(cs);
            connection.Open();
            var command = new SqlCommand("SELECT name FROM sys.databases WHERE database_id > 4", connection);

            await using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                var name = reader.GetString(0);
                var database = Database.Create(name);
                result.Add(database);
            }
        }

        return result;
    }

    public async Task<List<Table>> GetTables(Guid envId, Guid dbId)
    {
        var result = new List<Table>();
        var connectionString = _environmentService.GetConnectionString(envId);

        if (connectionString.IsSome)
        {
            var database = _databaseService.GetById(dbId);
            var cs = string.Empty;
            connectionString.IfSome(c => cs = c);

            database.IfSome(o =>
            {
                var c = new SqlConnectionStringBuilder(cs)
                {
                    InitialCatalog = o.Name
                };
                cs = c.ConnectionString;
            });


            await using var connection = new SqlConnection(cs);
            connection.Open();
            var command = new SqlCommand("SELECT table_name FROM information_schema.tables WHERE table_type = 'BASE TABLE';", connection);

            await using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                var name = reader.GetString(0);
                var table = Table.Create(name);
                table.DatabaseId = envId;
                table.EnvironmentId = envId;
                result.Add(table);
            }
        }


        return result;
    }

    public async Task<bool> CheckConnection(Guid envId)
    {
        var connectionString = _environmentService.GetConnectionString(envId);
        if (!connectionString.IsSome) return false;

        var cs = string.Empty;
        connectionString.IfSome(c => cs = c);
        try
        {
            await using var connection = new SqlConnection(cs);
            connection.Open();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public Option<string> GetConnectionString(Guid envId, Guid dbId)
    {
        var connectionString = _environmentService.GetConnectionString(envId);
        if (connectionString.IsSome)
        {
            var cs = string.Empty;
            connectionString.IfSome(c => cs = c);
            var database = _databaseService.GetById(dbId);

            return database.Match(db =>
            {
                var c = new SqlConnectionStringBuilder(cs)
                {
                    InitialCatalog = db.Name
                };
                return c.ConnectionString;
            }, () => string.Empty);
        }

        return connectionString;
    }

    public async Task<IEnumerable<Column>> GetColumns(Guid envId, Guid dbId, Guid tableId)
    {
        var result = new List<Column>();
        var connectionString = _environmentService.GetConnectionString(envId);

        if (connectionString.IsSome)
        {
            var database = _databaseService.GetById(dbId);
            var table = _tableService.GetById(tableId);
            if (table.IsNone) return result;

            var cs = string.Empty;
            connectionString.IfSome(c => cs = c);

            database.IfSome(o =>
            {
                var c = new SqlConnectionStringBuilder(cs)
                {
                    InitialCatalog = o.Name
                };
                cs = c.ConnectionString;
            });


            await using var connection = new SqlConnection(cs);
            connection.Open();
            var tableName = table.Map(x => x.Name);

            var query = $@"SELECT
    COLUMN_NAME,
    DATA_TYPE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = '{tableName}'";

            var command = new SqlCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                var name = reader.GetString(0);
                var type = reader.GetString(1);
                var column = new Column
                {
                    Id = Guid.NewGuid(),
                    EnvironmentId = envId,
                    DatabaseId = dbId,
                    TableId = tableId,
                    Type = type,
                    Name = name,
                    IsActive = true
                };

                result.Add(column);
            }
        }


        return result;
    }
}