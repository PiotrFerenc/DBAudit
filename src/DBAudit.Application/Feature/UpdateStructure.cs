using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.DatabaseProvider;
using DBAudit.Infrastructure.Storage;
using Microsoft.Data.SqlClient;

namespace DBAudit.Application.Feature;

public class UpdateStructure : IRequest
{
    public Guid EnvId { get; set; }
}

public class UpdateStructureHandler(IDatabaseProvider databaseProvider, IDatabaseService databaseService, ITableService tableService, IColumnService columnService, IEnvironmentService environmentService) : ICommandHandler<UpdateStructure>
{
    public async Task HandleAsync(UpdateStructure command)
    {
        var envId = command.EnvId;
        await environmentService.GetConnectionString(envId).IfSomeAsync(async cs =>
        {
            var connection = new SqlConnection(cs.ToString());
            await connection.OpenAsync();

            var databases = await databaseProvider.GetDatabases(connection);

            foreach (var name in databases)
            {
                Database db = null;
                databaseService.GetByName(envId, name).Match(
                    d => db = d,
                    () =>
                    {
                        db = Database.Create(name);
                        db.EnvironmentId = envId;
                        databaseService.Add(db);
                    });

                connection.ChangeDatabase(db.Name);
                var tables = await databaseProvider.GetTables(connection);

                foreach (var table in tables)
                {
                    Table dbTable = null;
                    tableService.Get(db.Id, db.EnvironmentId, table).Match(
                        t => dbTable = t,
                        () =>
                        {
                            dbTable = Table.Create(table);
                            dbTable.DatabaseId = db.Id;
                            dbTable.EnvironmentId = envId;
                            tableService.Add(dbTable);
                        });

                    var columns = await databaseProvider.GetColumns(dbTable.Name, connection);

                    foreach (var column in columns)
                    {
                        columnService.GetByName(dbTable.Id, column.Name).IfNone(() => columnService.Add(new Column
                        {
                            Id = Guid.NewGuid(),
                            Name = column.Name,
                            IsActive = true,
                            Type = column.Type,
                            TableId = dbTable.Id,
                            EnvironmentId = envId,
                            DatabaseId = db.Id,
                        }));
                    }
                }
            }
        });
    }
}