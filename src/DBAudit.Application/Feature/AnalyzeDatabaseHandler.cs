using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.DatabaseProvider;
using DBAudit.Infrastructure.Queue;
using DBAudit.Infrastructure.Storage;
using Microsoft.Data.SqlClient;

namespace DBAudit.Application.Feature;

public class AnalyzeDatabaseHandler(IDatabaseProvider databaseProvider, IQueueProvider queryProvider, IReportService reportService, IDatabaseService databaseService) : ICommandHandler<AnalyzeDatabase>
{
    public Task HandleAsync(AnalyzeDatabase message)
    {
        var cs = databaseProvider.GetConnectionString(message.EnvId, message.DbId);
        cs.IfSome(connectionString =>
        {
            var databases = databaseService.GetAll(message.DbId);

            var connection = new SqlConnection(connectionString);
            reportService.Remove(message.DbId);
            reportService.Add(new ReportView
            {
                DatabaseId = message.DbId,
                EnvId = message.EnvId,
                Title = "123",
                Links = databases.Select(x => (x.Name, $"/database/{x.Id}")).ToList(),
                Counters = []
            });

            queryProvider.Enqueue(new CounterMetricMessage(connection, message.DbId));
        });

        return Task.CompletedTask;
    }
}