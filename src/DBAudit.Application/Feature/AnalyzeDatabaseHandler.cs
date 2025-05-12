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
            var databases = databaseService.GetAll(message.EnvId);

            var connection = new SqlConnection(connectionString);
            reportService.Remove(message.DbId);
            var report = new ReportView
            {
                Id = Guid.NewGuid(),
                DatabaseId = message.DbId,
                EnvId = message.EnvId,
                Title = "Env metrics",
                Links = databases.Select(x => (x.Name, $"/database/{x.Id}")).ToList(),
                Counters = []
            };
            reportService.Add(report);

            queryProvider.Enqueue(new CounterMetricMessage(connection, report.Id));
        });

        return Task.CompletedTask;
    }
}