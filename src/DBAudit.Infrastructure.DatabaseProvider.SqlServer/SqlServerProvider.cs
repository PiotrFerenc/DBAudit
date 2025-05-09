using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Storage;
using LanguageExt;
using Microsoft.Data.SqlClient;

namespace DBAudit.Infrastructure.DatabaseProvider.SqlServer;

public class SqlServerProvider : IDatabaseProvider
{
    private readonly IQueryService _queryService;
    private readonly IEnvironmentService _environmentService;
    private readonly IDatabaseService _databaseService;
    private readonly ITableService _tableService;

    public SqlServerProvider(IQueryService queryService, IEnvironmentService environmentService, IDatabaseService databaseService, ITableService tableService)
    {
        _queryService = queryService;
        _environmentService = environmentService;
        _databaseService = databaseService;
        _tableService = tableService;
    }

    public async Task<List<string>> GetDatabases(Guid envId)
    {
        var result = new List<string>();

        await _environmentService.GetConnectionString(envId).IfSomeAsync(async cs =>
        {
            await using var connection = new SqlConnection(cs.ToString());
            result = await _queryService.QueryData(connection, "SELECT name FROM sys.databases WHERE database_id > 4", r => r.GetString(0));
        });


        return result;
    }

    public async Task<List<Table>> GetTables(Guid envId, Guid dbId)
    {
        var result = new List<Table>();

        await _environmentService.GetConnectionString(envId).IfSomeAsync(cs =>
            _databaseService.GetById(dbId).IfSomeAsync(async db =>
            {
                cs.InitialCatalog = db.Name;
                await using var connection = new SqlConnection(cs.ToString());

                result = await _queryService.QueryData(connection, "SELECT table_name FROM information_schema.tables WHERE table_type = 'BASE TABLE';", r =>
                {
                    var name = r.GetString(0);
                    var table = Table.Create(name);
                    table.DatabaseId = envId;
                    table.EnvironmentId = envId;

                    return table;
                });
            })
        );

        return result;
    }

    public async Task<bool> CheckConnection(Guid envId)
    {
        return true;
    }

    public Option<string> GetConnectionString(Guid envId, Guid dbId) => _environmentService.GetConnectionString(envId).Match(c => c.ToString(), () => Option<string>.None);

    public async Task<IEnumerable<Column>> GetColumns(Guid envId, Guid dbId, Guid tableId)
    {
        var result = new List<Column>();

        await _environmentService.GetConnectionString(envId).IfSomeAsync(cs =>
            _databaseService.GetById(dbId).IfSomeAsync(async db =>
            {
                cs.InitialCatalog = db.Name;
                await using var connection = new SqlConnection(cs.ToString());
                var tableName = _tableService.GetById(tableId).Match(x => x.Name, () => string.Empty);
                result = await _queryService.QueryData(connection, $@"SELECT COLUMN_NAME,    DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'", reader =>
                {
                    var name = reader.GetString(0);
                    var type = reader.GetString(1);

                    return new Column
                    {
                        Id = Guid.NewGuid(),
                        EnvironmentId = envId,
                        DatabaseId = dbId,
                        TableId = tableId,
                        Type = type,
                        Name = name,
                        IsActive = true
                    };
                });
            })
        );

        return result;
    }
}