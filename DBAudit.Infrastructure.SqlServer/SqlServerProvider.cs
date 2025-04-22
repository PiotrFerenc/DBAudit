using DBAudit.Infrastructure.Data.Entities;
using DBAudit.Infrastructure.Repositories;
using Microsoft.Data.SqlClient;

namespace DBAudit.Infrastructure.SqlServer;

public class SqlServerProvider : IDatabaseProvider
{
    private readonly IEnvironmentService _environmentService;
    private readonly IDatabaseService _databaseService;

    public SqlServerProvider(IEnvironmentService environmentService, IDatabaseService databaseService)
    {
        _environmentService = environmentService;
        _databaseService = databaseService;
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
            var database = _databaseService.GetById(dbId.ToString());
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

        return false;
    }
}