using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.DatabaseProvider;
using DBAudit.Infrastructure.Queue;
using DBAudit.Infrastructure.Storage;

namespace DBAudit.Application.Feature;

public class UpdateEnvironmentHandler(IDatabaseProvider databaseProvider, IDatabaseService databaseService, IQueueProvider queueProvider) : ICommandHandler<EnvironmentMessage>
{
    public async Task HandleAsync(EnvironmentMessage command)
    {
        var items = await databaseProvider.GetDatabases(command.Id);

        foreach (var database in items)
        {
            if (!databaseService.Exist(command.Id, database.Name))
            {
                database.EnvironmentId = command.Id;
                databaseService.Add(database);
            }

            queueProvider.Enqueue(new DatabaseMessage(command.Id, database.Id));
        }
    }
}