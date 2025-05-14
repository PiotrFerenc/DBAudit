using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.DatabaseProvider;
using DBAudit.Infrastructure.Queue;
using DBAudit.Infrastructure.Storage;

namespace DBAudit.Application.Feature;

public class UpdateDatabasesHandler(IDatabaseProvider databaseProvider, IDatabaseService databaseService,  ICommandDispatcher dispatcher) : ICommandHandler<EnvironmentMessage>
{
    public async Task HandleAsync(EnvironmentMessage command)
    {
        var databases = await databaseProvider.GetDatabases(command.Id);

        foreach (var name in databases)
        {
            var dbId = Guid.Empty;
            databaseService.GetByName(command.Id, name).Match(d => dbId = d.Id,
                () =>
                {
                    var database = Database.Create(name);
                    database.EnvironmentId = command.Id;
                    dbId = database.Id;
                    databaseService.Add(database);
                });
            await dispatcher.Send(new DatabaseMessage(command.Id, dbId));
        }
    }
}