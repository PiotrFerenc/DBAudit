using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.DatabaseProvider;
using DBAudit.Infrastructure.Queue;
using DBAudit.Infrastructure.Storage;

namespace DBAudit.Application.Feature;

public class UpdateEnvironmentHandler(IDatabaseProvider databaseProvider, IDatabaseService databaseService, IQueueProvider queueProvider, IReportService reportService) : ICommandHandler<EnvironmentMessage>
{
    public async Task HandleAsync(EnvironmentMessage command)
    {
        var items = await databaseProvider.GetDatabases(command.Id);

        foreach (var name in items)
        {
            var dbId = Guid.Empty;
            databaseService.GetByName(command.Id, name).Match(d => dbId = d.Id,
                () =>
                {
                    var database = Database.Create(name);
                    database.EnvironmentId = command.Id;
                    dbId = database.Id;
                    databaseService.Add(database);

                    reportService.Remove(database.Id);
                    reportService.Add(new ReportView
                    {
                        DatabaseId = database.Id,
                        Title = "123",
                        Links = [],
                        Counters = []
                    });
                });

            queueProvider.Enqueue(new DatabaseMessage(command.Id, dbId));
        }
    }
}